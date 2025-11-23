using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action_ARPG.Health
{
    public abstract class CharacterHealthBase : MonoBehaviour
    {
        //共同的受伤函数
        //共同的格挡逻辑函数
        
        protected Transform currentAttacker;
        protected Animator animator;

        [SerializeField, Header("体力值")] protected float enduranceValue;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void OnEnable()
        {
            GameEventManager.MainInstance.AddEventListening<float,string,string,Transform,Transform>("HitEvent",OnChanracterHitEventHandler);
        }

        protected virtual void OnDisable()
        {
            GameEventManager.MainInstance.RemoveEventListening<float,string,string,Transform,Transform>("HitEvent",OnChanracterHitEventHandler);
        }

        protected virtual void CharacterHitAction(float damage,string hitName, string parryName)
        {
            
        }

        protected void TakeDamage(float damage)
        {
            
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

        private void OnChanracterHitEventHandler(float damage, string hitName, string parryName, Transform attacker,
            Transform self)
        {
            if(self != transform) return;


            SetAttacker(attacker);
            CharacterHitAction(damage,hitName, parryName);
            TakeDamage(damage);
        }
       
    }
}
