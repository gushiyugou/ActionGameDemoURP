using System;
using System.Collections;
using System.Collections.Generic;
using Action_ARPG;
using Action_ARPG.ComboData;
using MyAssets.Scripts.Tools;
using UnityEngine;

public class PlayerComboCotrol : MonoBehaviour
{
    /*
     *1.存储连招的的容器
     * 2.当前使用的招式
     */
    private Animator animator;
    [SerializeField] private CharacterCombo_SO normalCombo;
    private CharacterCombo_SO currentCombo;

    private int currentComboActionIndex;
    private int hitIndex;
    private float maxColdTime;
    private bool canAttackInput;
    
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


        return true;
    }

    private void CharacterNormalAttack()
    {
        if(!CanNormalAttackInput()) return;

        if (GameInputManager.MainInstance.LAttack)
        {
            if (currentCombo != null && currentCombo != normalCombo)
            {
                currentCombo = normalCombo;
                ResetComboInfo();
            }

            ExecuteComboAction();
        }
    }


    public void ExecuteComboAction()
    {
        hitIndex = 0;
        if (currentComboActionIndex == currentCombo.GetComboMaxCount())
        {
            currentComboActionIndex = 0;
        }

        maxColdTime = currentCombo.GetComboColdTime(currentComboActionIndex);
        PlayAnimation(currentCombo.GetOneComboAction(currentComboActionIndex));
        Debug.Log(currentCombo.GetOneComboAction(currentComboActionIndex));
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
    
    

    #endregion


    #region 动画相关

    private void PlayAnimation(string animationName, float transitTime = 0.25f, int layer = 0,int fixedTimeOffset = 0)
    {
        animator.CrossFadeInFixedTime(animationName,transitTime,layer,fixedTimeOffset);
    }
    

    #endregion
}
