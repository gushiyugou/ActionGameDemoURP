using System;
using System.Collections;
using System.Collections.Generic;
using Action_ARPG;
using Action_ARPG.ComboData;
using GGG.Tool;
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
    private Transform currentEnemy;
    [SerializeField,Header("普通攻击连招表")] private CharacterCombo_SO normalCombo;
    [SerializeField,Header("强化攻击连招表")] private CharacterCombo_SO intensifiedAttack;
    [SerializeField,Header("攻击时方向旋转速度")] float rotationVelocity;
    private CharacterCombo_SO currentCombo;
    

    private int currentComboActionIndex;
    private int hitIndex;
    private int currentAttackNumber;
    private float maxColdTime;
    private bool canAttackInput;
     

    //技能状态标识
    public bool canChangeState;
    
    //检测方向
    private Vector3 detectionDirection;
    [SerializeField,Header("攻击检测相关参数")] private float detectionRange;
    [SerializeField] private float detectionDistance;

    
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (Camera.main != null) cameraGameObject = Camera.main.transform;
    }

    private void Start()
    {
        canAttackInput = true;
        currentCombo = normalCombo;
    }

    private void Update()
    {
        UpdateDetectionDirection();
        CharacterNormalAttack();
        NormalAttackEnd();
        LookTargetOnAttack();
    }

    private void FixedUpdate()
    {
        
        AttackCheckTag();
    }


    #region AttackCheck

    private void AttackTrigger()
    {
        UpdateHitIndex();
        TriggerDamage();
        GamePoolManager.MainInstance.GetItem("AttackSound",transform.position, Quaternion.identity);
    }

    private void TriggerDamage()
    {
        if(currentEnemy==null) return;
        if(Vector3.Dot(transform.forward,DevelopmentToos.DirectionForTarget(transform,currentEnemy)) <0.85f) return;
        if (DevelopmentToos.DistanceForTarget(currentEnemy,transform) >1.3f) return;
        Debug.Log("攻击伤害触发");
        if (animator.AnimationAtTag("Attack"))
        {
            GameEventManager.MainInstance.CallEvent("HitEvent",currentCombo.GetComboDamage(currentComboActionIndex),
                currentCombo.GetHitName(currentComboActionIndex,hitIndex),
                currentCombo.GetParryName(currentComboActionIndex,hitIndex),
                transform,currentEnemy);
        }
        else
        {
            //一般攻击状态下，而是其他带有特殊效果的动作
        }
        
    }


    private void AttackCheckTag()
    {
        if (Physics.SphereCast(transform.position + (transform.up * 0.7f), 
                detectionRange, detectionDirection, out var hitInfo,
                detectionDistance, 1<<9, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(0);
            currentEnemy = hitInfo.transform; 
        }
    }
    
    private void UpdateDetectionDirection()
    {
        detectionDirection = (cameraGameObject.forward * GameInputManager.MainInstance.MovementInput.y) +
                             (cameraGameObject.right * GameInputManager.MainInstance.MovementInput.x);
        detectionDirection.Set(detectionDirection.x, 0, detectionDirection.z);
        detectionDirection = detectionDirection.normalized;
    }

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
        currentComboActionIndex++;
        if (currentComboActionIndex == currentCombo.GetComboMaxCount())
        {
            currentComboActionIndex = 0;
        }

        maxColdTime = currentCombo.GetComboColdTime(currentComboActionIndex);
        canChangeState = false;
        PlayAnimation(currentCombo.GetOneComboAction(currentComboActionIndex));
        TimerManager.MainInstance.TryGetOneTimer(maxColdTime,UpdateComboInfo);
        canAttackInput = false;
    }

    private void UpdateComboInfo()
    {
        
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
        if (animator.AnimationAtTag("motion")&& canAttackInput)
            ResetComboInfo();
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
        if (animator.AnimationAtTag("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5 && currentEnemy != null)
        {
            transform.Look(currentEnemy.position,50f);
            // transform.rotation = Quaternion.LookRotation(currentEnemy.position);
        }
           
    }

    #region 更新受伤索引

    private void UpdateHitIndex()
    {
        hitIndex++;
        if (hitIndex >= currentCombo.GetComboHitMaxCount(currentComboActionIndex))
            hitIndex = 0;
        
    }
    

    #endregion
    
    #endregion

    #region 特殊攻击逻辑

    private bool AllowSpecialAttack()
    {
        if(animator.AnimationAtTag("Finish")) return false;
        if (currentCombo == null) return false;

        return true;
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
    }

    #endregion
}
