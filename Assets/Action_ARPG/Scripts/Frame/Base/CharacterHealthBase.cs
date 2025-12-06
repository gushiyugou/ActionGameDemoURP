using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;

namespace Action_ARPG.Health
{
    public abstract class CharacterHealthBase : MonoBehaviour
    {
        //共同的受伤函数
        //共同的格挡逻辑函数
        
        protected Transform currentAttacker;
        protected Animator animator;

        [SerializeField,Header("角色健康信息")] protected CharacterHealthInfo_SO healthInfo;
        

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (healthInfo == null)
            {
                Debug.LogError($"{gameObject.name}没有初始化HealthInfo，请赋值检查");
                return;
            }
            healthInfo.InitCharacterHealthInfo();
        }

        protected virtual void OnEnable()
        {
            GameEventManager.MainInstance.AddEventListening<float,string,string,Transform,Transform>("HitEvent",OnChanracterHitEventHandler);
            GameEventManager.MainInstance.AddEventListening<string,string,Transform,Transform>("SpacialAttackHitEvent",SpacialAttackHitEventHandler);
            GameEventManager.MainInstance.AddEventListening<float,Transform>("CalculateDamage",SpacialAttackTakeDamage);
        }

        protected virtual void OnDisable()
        {
            GameEventManager.MainInstance.RemoveEventListening<float,string,string,Transform,Transform>("HitEvent",OnChanracterHitEventHandler);
            GameEventManager.MainInstance.RemoveEventListening<string,string,Transform,Transform>("SpacialAttackHitEvent",SpacialAttackHitEventHandler);
            GameEventManager.MainInstance.RemoveEventListening<float,Transform>("CalculateDamage",SpacialAttackTakeDamage);
        }

        protected virtual void Update()
        {
            // OnHitLookAttacker();
        }

        
        /// <summary>
        /// 敌人的受伤动画存在差异性，所以声明为虚方法，由子类去实现具体的受伤逻辑
        /// </summary>
        /// <param name="damage">伤害值</param>
        /// <param name="hitName">受伤动画名</param>
        /// <param name="parryName">格挡动画名</param>
        protected virtual void CharacterHitAction(float damage,string hitName, string parryName)
        {
            
        }
        
        protected virtual void SpacialAttackHitAction(string hitName, string parryName)
        {
            
        }

        protected void TakeDamage(float damage)
        {
            healthInfo.DamageToHP(damage);
        }

        protected void SpacialAttackTakeDamage(float damage, Transform self)
        {
            if(self!= transform) return;
            
            TakeDamage(damage);
            GamePoolManager.MainInstance.GetItem("HitSound",transform.position, Quaternion.identity);
        }
        
        /// <summary>
        /// 设置当前的攻击者
        /// </summary>
        /// <param name="attacker"></param>
        private void SetAttacker(Transform attacker)
        {   
            if(currentAttacker == null || currentAttacker != attacker)
                currentAttacker = attacker;
        }

        private void OnHitLookAttacker()
        {
            if(currentAttacker == null )return;
            // if ((animator.AnimationAtTag("Hit") || animator.AnimationAtTag("Parry")) &&
            //     animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            // {
            //     transform.Look(currentAttacker.position,50f);
            //     
            // }
        }

        #region 受伤事件
        private void OnChanracterHitEventHandler(float damage, string hitName, string parryName, Transform attacker,
            Transform self)
        {
            if(self != transform) return;
            
            SetAttacker(attacker);
            CharacterHitAction(damage,hitName, parryName);
            
        }
        
        private void SpacialAttackHitEventHandler(string hitName, string parryName, Transform attacker,
            Transform self)
        {
            if (self == transform)
            {
                SetAttacker(attacker);
                // SpacialAttackHitAction(damage,hitName, parryName);
                animator.Play(hitName,-1,0f);
                // SpacialAttackTakeDamage(damage);
            }
        }
         
        #endregion
        
        
        
       
    }
}
