using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;

namespace Action_ARPG.Health
{
    public class EnemyHealthControl : CharacterHealthBase
    {
        
        //临时的
        private int vit = 0;
        protected override void CharacterHitAction(float damage,string hitName, string parryName)
        {
            //TODO:格挡和受伤逻辑
            //1.判断体力值是否大于0，如果大于0，则可以进行格挡
            //2.如果传入的伤害值大于了一个格挡阈值，则格挡失效，进行受伤，也就是破防状态，会扣除大量的体力值或者直接扣除生命值
            //3.如果传入的伤害值小于格挡阈值，则格挡成功，扣除一定的体力值，体力值小于0则格挡失效
            if (healthInfo.IsEnergyFull)
            {
                //格挡
                if (!animator.AnimationAtTag("Attack") && damage <=30f)
                {
                    animator.Play(parryName,-1, 0f);
                    //获取音效
                    GamePoolManager.MainInstance.GetItem("BlockSound", transform.position, Quaternion.identity);
                    healthInfo.DamageToEnergy(damage);
                    //TODO:被特殊攻击攻击时，通知敌人
                    if (!healthInfo.IsEnergyFull)
                    {
                        GameEventManager.MainInstance.CallEvent<bool>("ActiveSpecialEvent",true);
                    }
                    
                }
            }
            else
            {
                if(healthInfo.CurrentHP < 20)
                    GameEventManager.MainInstance.CallEvent<bool>("ActiveSpecialEvent",true);
                
                animator.Play(hitName,-1,0f);
                TakeDamage(damage);
                //获取音效
                GamePoolManager.MainInstance.GetItem("HitSound", transform.position, Quaternion.identity);
            }
        }


        
        

        
    }
}
