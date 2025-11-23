using System.Collections;
using System.Collections.Generic;
using Action_ARPG;
using Action_ARPG.ComboData;
using MyAssets.Scripts.Tools;
using UnityEngine;




public class PlayerComboControl : MonoBehaviour
{
    /*
     *1.存储连招的的容器
     * 2.当前使用的招式
     */
    private Animator animator;
    [SerializeField,Header("普通攻击连招表")] private CharacterCombo_SO normalCombo;
    [SerializeField,Header("强化攻击连招表")] private CharacterCombo_SO intensifiedAttack;
    private CharacterCombo_SO currentCombo;

    private int currentComboActionIndex;
    private int hitIndex;
    private int currentAttackNumber;
    private float maxColdTime;
    private bool canAttackInput;
     

    //技能状态标识
    public bool canChangeState;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        canAttackInput = true;
        currentCombo = normalCombo;
    }

    private void Update()
    {
        CharacterNormalAttack();
        NormalAttackEnd();

    }

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



        return true;
    }


    private void DashStateAttack()
    {
        currentComboActionIndex = Random.Range(0, currentCombo.GetComboMaxCount());
    }

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
            Debug.Log(currentAttackNumber);
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
    
    #endregion

    #region 状态的切换
    

    #endregion

    #region 动画相关

    private void PlayAnimation(string animationName, float transitTime = 0.25f, int layer = 0,int fixedTimeOffset = 0)
    {
        animator.CrossFadeInFixedTime(animationName,transitTime,layer,fixedTimeOffset);
    }
    
    #endregion
}
