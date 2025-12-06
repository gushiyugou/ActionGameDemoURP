using System;
using System.Collections;
using System.Collections.Generic;
using Action_ARPG;
using Action_ARPG.ComboData;
using GGG.Tool;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerComboControl : MonoBehaviour
{
    /*
     *1.存储连招的的容器
     * 2.当前使用的招式
     */
    private Animator animator;
    private Transform cameraGameObject;
    private Transform currentAttackLockTarget;
    [SerializeField,Header("普通攻击连招表")] private CharacterCombo_SO normalCombo;
    [SerializeField,Header("强化攻击连招表")] private CharacterCombo_SO intensifiedAttack;
    [SerializeField,Header("特殊技能表")] private CharacterCombo_SO specialAttack;
    [SerializeField,Header("暗杀技能表")] private CharacterCombo_SO assassinateAttack;
    [SerializeField,Header("攻击时方向旋转速度")] float rotationVelocity;
    [SerializeField,Header("旋转的模型")] Transform rotationModel;
    private CharacterCombo_SO currentCombo;
    

    private int currentComboActionIndex;
    private int hitIndex;
    private int currentAttackNumber;
    private float maxColdTime;
    private bool canAttackInput;
    private bool isCanIntensifiedAttack = false;
    private Collider[] attackUnits;


    private int specialAttackIndex;
    private bool canFinishAttack;
     

    //技能状态标识
    public bool canChangeState;
    
    //检测方向
    private Vector3 detectionDirection;
    [SerializeField,Header("攻击检测相关参数")] private float detectionRange;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float centerDetectionRadius;
    [SerializeField] private LayerMask detectionLayer;

    
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (Camera.main != null) cameraGameObject = Camera.main.transform;
    }

    private void Start()
    {
        canAttackInput = true;
        currentCombo = normalCombo;
        canFinishAttack = false;
    }

    private void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListening<bool>("ActiveSpecialEvent",EnableFinishAttack);
    }

    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEventListening<bool>("ActiveSpecialEvent",EnableFinishAttack);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < normalCombo.GetComboMaxCount(); i++)
            {
                Debug.Log(normalCombo.GetOneComboAction(i));
            }
        }
        // UpdateDetectionDirection();
        LookTargetOnAttack();
        GetOneAttackUnit();
        CharacterNormalAttack();
        SpecialAttackInput();
        AssassinateAttackInput();
        NormalAttackEnd();
        MatchPosition();
        
        UpdateEndAnimation();

        // ClearCurrentEnemy();

    }

    private void FixedUpdate()
    {
        
        // AttackCheckTag();
        GetNearAllAttackUnit();
    }


    #region 位置同步

    private void MatchPosition()
    {
        if(currentAttackLockTarget == null ) return;
        if (!animator) return;
        if (animator.AnimationAtTag("Finish") && !animator.IsInTransition(0))
        {
            // transform.Look(currentEnemy.position,500f);
            // currentEnemy.Look(transform.position,500f);
            // transform.position = currentEnemy.position;
            transform.rotation = Quaternion.LookRotation(-currentAttackLockTarget.forward);
            RuningMatchPosition(specialAttack,specialAttackIndex);    
        }else if(animator.AnimationAtTag("Assassinate"))
        {
            transform.rotation = Quaternion.LookRotation(currentAttackLockTarget.forward);
            RuningMatchPosition(assassinateAttack,specialAttackIndex, 0f,0.25f);
        }
        
    }
    /// <summary>
    /// 动画匹配位置同步
    /// </summary>
    private void RuningMatchPosition(CharacterCombo_SO comboSO,int index,float startTime = 0f,float endTime = 0.15f)
    {
        if (!animator.isMatchingTarget && !animator.IsInTransition(0))
        {
            //animator.IsInTransition(0)判断当前动画是否处于过度状态
            //animator.IsMatchingTarget判断当前动画是否处于匹配状态
            //动画匹配函数：MatchTarget:参数：匹配的目标位置、匹配的目标旋转、匹配的部位、权重掩码、开始时间、结束时间
            animator.MatchTarget(currentAttackLockTarget.position+(-transform.forward*comboSO.GetComboPositionOffset(index)),
                Quaternion.identity, AvatarTarget.Body,
                new MatchTargetWeightMask(Vector3.one,0f),startTime,endTime
            );
        }
        
    }

    #endregion

    #region AttackCheck

    private void AttackTrigger()
    {
        UpdateHitIndex();
        TriggerDamage();
        GamePoolManager.MainInstance.GetItem("AttackSound",transform.position, Quaternion.identity);
    }

    private void TriggerDamage()
    {
        if(currentAttackLockTarget==null) return;
        if(Vector3.Dot(transform.forward,DevelopmentToos.DirectionForTarget(transform,currentAttackLockTarget)) <0.85f) return;
        if (DevelopmentToos.DistanceForTarget(currentAttackLockTarget,transform) >1.3f) return;
        if (animator.AnimationAtTag("Attack"))
        {
            GameEventManager.MainInstance.CallEvent<float,string,string,Transform,Transform>("HitEvent",currentCombo.GetComboDamage(currentComboActionIndex),
                currentCombo.GetHitName(currentComboActionIndex,hitIndex),
                currentCombo.GetParryName(currentComboActionIndex,hitIndex),
                transform,currentAttackLockTarget);
        }
        else
        {
            //一般攻击状态下，而是其他带有特殊效果的动作
            GameEventManager.MainInstance.CallEvent<float,Transform>("CalculateDamage",specialAttack.GetComboDamage(specialAttackIndex),currentAttackLockTarget);
        }
        
    }


    private void AttackCheckTag()
    {
        if (Physics.SphereCast(transform.position + (transform.up * 0.7f), 
                detectionRange, detectionDirection, out var hitInfo,
                detectionDistance, 1<<9, QueryTriggerInteraction.Ignore))
        {
            currentAttackLockTarget = hitInfo.transform; 
        }
    }
    
    private void UpdateDetectionDirection()
    {
        detectionDirection = (cameraGameObject.forward * GameInputManager.MainInstance.MovementInput.y) +
                             (cameraGameObject.right * GameInputManager.MainInstance.MovementInput.x);
        detectionDirection.Set(detectionDirection.x, 0, detectionDirection.z);
        detectionDirection = detectionDirection.normalized;
    }

    #region 范围检测敌人相关
    //1.检测以自身为中心，一定范围内的所有敌人，同时取其中距离最近的人
    //2.在当前玩家没有目标的情况下，取最近的敌人作为当前目标
    //3.自选:
    //（1）当前有目标，不再更新目标，直到当前目标消失或者距离太远
    //（2）当前目标只要大于了一定距离就更新
    private void GetNearAllAttackUnit()
    {
        // if(currentAttackLockTarget != null) return;
        if(currentAttackLockTarget != null && 
           DevelopmentToos.DistanceForTarget(currentAttackLockTarget.transform,transform) 
           < centerDetectionRadius) return;
        attackUnits =  Physics.OverlapSphere(transform.position + (transform.up * 0.7f),
            centerDetectionRadius, detectionLayer, QueryTriggerInteraction.Ignore);
    }

    private void GetOneAttackUnit()
    {
        if (attackUnits.Length == 0)
        {
            ClearCurrentEnemy();
            return;
        }
        // if(!animator.AnimationAtTag("Attack")) return;
        if(currentAttackLockTarget != null && 
           DevelopmentToos.DistanceForTarget(currentAttackLockTarget.transform,transform) 
           < 1.2f) return;
        // if(currentAttackLockTarget != null) return;
        QuickSort(attackUnits,0,attackUnits.Length-1);
        if (!canFinishAttack)
        {
            currentAttackLockTarget = attackUnits[0].transform != null?attackUnits[0].transform:currentAttackLockTarget;
        }
        
        
        // Transform enemy = null;
        // int provit = attackUnits.Length/2;
        // float provitDistance = DevelopmentToos.DistanceForTarget(attackUnits[provit].transform, transform);
        // // float distance = Mathf.Infinity;//Mathf.Infinity表示无穷大数
        // for (int i = 0; i < attackUnits.Length; i++)
        // {
        //     // float dis = DevelopmentToos.DistanceForTarget(attackUnits[i].transform, transform);
        //     // if (dis >= provitDistance)
        //     // {
        //     //     
        //     // }
        //     
        // }
    }


    /// <summary>
    /// 清空当前锁定的敌人
    /// </summary>
    /// <returns></returns>
    private void ClearCurrentEnemy()
    {
        if(currentAttackLockTarget == null) return;
        if (animator.GetFloat(AnimationID.MovementID) > 0.7f)
        {
            canFinishAttack = false;
            // currentAttackLockTarget = null;
        }
    }
    

    #endregion
    
    

    #endregion
    
    #region 角色的基础攻击

    private bool CanNormalAttackInput()
    {
        /*
         *1.不允许攻击输入
         *2.角色当前是受击状态
         * 3.角色处决时
         * 4.角色格挡时
         * 几种基本情况下不允许普通攻击，后续有逻辑时再加
         */
        if (!canAttackInput) return false;
        if (animator.AnimationAtTag("Hit")) return false;
        if (animator.AnimationAtTag("Parry")) return false;
        if (animator.AnimationAtTag("Dash")) return false;
        if (animator.AnimationAtTag("Finish")) return false;



        return true;
    }


    /// <summary>
    /// 冲刺或者霸体攻击状态下
    /// </summary>
    private void DashStateAttack()
    {
        currentComboActionIndex = Random.Range(0, currentCombo.GetComboMaxCount());
    }

    /// <summary>
    /// 角色普通攻击逻辑
    /// </summary>
    private void CharacterNormalAttack()
    {
        
        if(!CanNormalAttackInput()) return;

        if (GameInputManager.MainInstance.LAttack)
        {
            if (currentCombo != null && currentCombo != normalCombo)
            {
                ChangeCurrentCombo(normalCombo);
            }
            ExecuteComboAction();
        }
        else if (GameInputManager.MainInstance.RAttack)
        {
            isCanIntensifiedAttack = true;
            if (currentAttackNumber >= 3)
            {
                
                ChangeCurrentCombo(intensifiedAttack);
                if (intensifiedAttack.GetComboMaxCount() == 0)
                {
                    animator.SetBool(AnimationID.CanChangeID,canChangeState);
                    return;
                }
                switch (currentAttackNumber)
                {
                    case 3:
                        currentComboActionIndex = 0;
                        break;
                    case 4:
                        currentComboActionIndex = 1;
                        break;
                    case 5:
                        currentComboActionIndex = 2;
                        break;
                }
            }
            else
            {
                animator.SetBool(AnimationID.CanChangeID,canChangeState);
                currentAttackNumber = 0;
                return;
            }
            currentAttackNumber = 0;
            ExecuteComboAction();
        }
        animator.SetBool(AnimationID.CanChangeID,canChangeState);
    }
    
    public void ExecuteComboAction()
    {
        //判断当前攻击状态是否是普通连招
        currentAttackNumber += (currentCombo == normalCombo) ? 1 : 0;
        hitIndex = 0;
        if (currentComboActionIndex == currentCombo.GetComboMaxCount() || currentComboActionIndex < 0)
        {
            currentComboActionIndex = 0;
        }

        maxColdTime = currentCombo.GetComboColdTime(currentComboActionIndex);
        canChangeState = false;
        PlayAnimation(currentCombo.GetOneComboAction(currentComboActionIndex));
        
        TimerManager.MainInstance.TryGetOneTimer(maxColdTime,UpdateComboInfo);
        canAttackInput = false;
        isCanIntensifiedAttack = false;
    }

    private void UpdateComboInfo()
    {
        currentComboActionIndex++;
        maxColdTime = 0f;
        canAttackInput = true;
    }
    private void ResetComboInfo()
    {
        currentComboActionIndex = 0;
        maxColdTime = 0;
    }

    private void NormalAttackEnd()
    {
        if (animator.AnimationAtTag("motion") && canAttackInput)
        {
            currentAttackNumber = 0;
            ResetComboInfo();
        }
            
    }
    
    private void ChangeCurrentCombo(CharacterCombo_SO comboSO)
    {
        if (currentCombo != comboSO)
        {
            currentCombo = comboSO;
            ResetComboInfo();
        }
    }
    
    
    private void LookTargetOnAttack()
    {
        
        if (animator.AnimationAtTag("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3 && currentAttackLockTarget != null)
        {
            if(Vector3.Distance(transform.position,currentAttackLockTarget.position) > 3f) return;
            Vector3 direction = currentAttackLockTarget.position - transform.position;
            // transform.Look(currentAttackLockTarget.position,500f);
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                // 使用插值而不是直接设置，限制旋转速度
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 50f);
            }
            
            // animator.MatchTarget(currentEnemy.position+(currentEnemy.forward*normalCombo.GetComboPositionOffset(currentComboActionIndex))
            //     ,Quaternion.identity,AvatarTarget.Body,new MatchTargetWeightMask(Vector3.one,0f),0,0.15f);
            // transform.rotation = Quaternion.LookRotation(currentEnemy.position);
        }
           
    }


    #region 结束动画播放判断

    private void UpdateEndAnimation()
    {
        if(GameInputManager.MainInstance.MovementInput != Vector2.zero || currentCombo.GetEndActionName(currentComboActionIndex) == "") return;
        if(currentComboActionIndex > currentCombo.GetComboMaxCount()) return;
        if(GameInputManager.MainInstance.LAttack || GameInputManager.MainInstance.RAttack) return;
        if (animator.AnimationAtTag("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //PlayAnimation(currentCombo.GetEndActionName(currentComboActionIndex));
            animator.Play(currentCombo.GetEndActionName(currentComboActionIndex),0,0.0f);
        }
    }


    #endregion

    #region 更新受伤索引

    private void UpdateHitIndex()
    {
        hitIndex++;
        if (hitIndex == currentCombo.GetComboHitMaxCount(currentComboActionIndex))
            hitIndex = 0;
    }
    

    #endregion
    
    #endregion

    #region 特殊攻击逻辑

    private bool AllowSpecialAttack()
    {
        if(animator.AnimationAtTag("Finish")) return false;
        if (currentCombo == null) return false;
        if (!canFinishAttack) return false;

        return true;
    }

    private void SpecialAttackInput()
    {
        //判断当前是否可执行特殊攻击
        if(!AllowSpecialAttack()) return;
        //执行特殊攻击逻辑
        if (GameInputManager.MainInstance.Grab && !animator.AnimationAtTag("Finish") && !animator.AnimationAtTag("Assassinate"))
        {
            //播放特殊必中攻击动画
            specialAttackIndex = Random.Range(0, specialAttack.GetComboMaxCount());
            PlayAnimation(specialAttack.GetOneComboAction(specialAttackIndex));
            //执行敌人的特殊攻击受伤逻辑
            GameEventManager.MainInstance.CallEvent<string,string,Transform,Transform>("SpacialAttackHitEvent",specialAttack.GetHitName(specialAttackIndex,0),
                specialAttack.GetParryName(specialAttackIndex,0),transform,currentAttackLockTarget);
            TimerManager.MainInstance.TryGetOneTimer(0.155f,UpdateComboInfo);
            ResetComboInfo();
        }
    }
    
    
    private void EnableFinishAttack(bool apply)
    {
        if (canFinishAttack) return;
        canFinishAttack = apply;
    }
    
    
    
    #endregion
    
    #region 暗杀攻击逻辑

    private bool AllowAssassinateAttack()
    {
        //1.距离太原
        //2.当前没有目标
        //3.当前正处于暗杀状态下
        //4.角度太太
        if (currentAttackLockTarget == null) return false;
        if(animator.AnimationAtTag("Assassinate")) return false;
        if(animator.AnimationAtTag("Finish")) return false;
        if (Vector3.Distance(transform.position, currentAttackLockTarget.position) > 5f) return false;
        if (Vector3.Angle(transform.position, currentAttackLockTarget.position) > 30f) return false;

        return true;
    }
    
    private void AssassinateAttackInput()
    {
        if(!AllowAssassinateAttack()) return;

        if (GameInputManager.MainInstance.TakeOut && !animator.AnimationAtTag("Finish")
            && !animator.AnimationAtTag("Assassinate"))
        {
            specialAttackIndex = Random.Range(0, assassinateAttack.GetComboMaxCount());
            PlayAnimation(assassinateAttack.GetOneComboAction(specialAttackIndex));
            //执行敌人的特殊攻击受伤逻辑
            GameEventManager.MainInstance.CallEvent<string,string,Transform,Transform>("SpacialAttackHitEvent",assassinateAttack.GetHitName(currentComboActionIndex,0),
                specialAttack.GetParryName(currentComboActionIndex,0),transform,currentAttackLockTarget);
            TimerManager.MainInstance.TryGetOneTimer(0.5f,UpdateComboInfo);
            ResetComboInfo();
            currentComboActionIndex = 0;
        }
    }
    
    #endregion
    
    
    #region 状态的切换
    

    #endregion

    #region 动画相关

    private void PlayAnimation(string animationName, float transitTime = 0.25f, int layer = 0,int fixedTimeOffset = 0)
    {
        animator.CrossFadeInFixedTime(animationName,transitTime,layer,fixedTimeOffset);
    }
    
    #endregion


    #region 可视化相关

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + (transform.up * 0.75f)+
                              (detectionDirection*detectionDistance), detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.up * 0.75f),centerDetectionRadius);
    }

    #endregion

    #region 快速排序函数,实现实时更新敌人距离
    
    public void QuickSort(Collider[] array,int leftIndex,int rightIndex)
    {
        if (leftIndex >= rightIndex) return;
        
        int tempLeftIndex, tempRightIndex;
        Collider tempValue = array[leftIndex];
        float benchmarkValue = DevelopmentToos.DistanceForTarget(tempValue.transform,transform);
        tempLeftIndex = leftIndex;
        tempRightIndex = rightIndex;
        while (tempLeftIndex != tempRightIndex)
        {
            while (tempLeftIndex < tempRightIndex && 
                   DevelopmentToos.DistanceForTarget(array[tempRightIndex].transform,transform)>benchmarkValue)
            {
                tempRightIndex--;
            }
            array[tempLeftIndex] = array[tempRightIndex];

            while (tempLeftIndex < tempRightIndex&& 
                   DevelopmentToos.DistanceForTarget(array[tempLeftIndex].transform,transform)<benchmarkValue)
            {
                tempLeftIndex++;
            }
            array[tempRightIndex] = array[tempLeftIndex];
        }
        array[tempRightIndex] = tempValue; 

        QuickSort(array,leftIndex,tempLeftIndex-1);
        QuickSort(array,tempLeftIndex+1,rightIndex);
        
    }
    

    #endregion
    
    
}