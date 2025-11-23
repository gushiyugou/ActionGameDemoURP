using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action_ARPG.Health
{
    public class EnemyHealthControl : CharacterHealthBase
    {
        
        protected override void CharacterHitAction(float damage,string hitName, string parryName)
        {
            //1.判断体力值是否大于0，如果大于0，则可以进行格挡
            //2.如果传入的伤害值大于了一个格挡阈值，则格挡失效，进行受伤，也就是破防状态，会扣除大量的体力值或者直接扣除生命值
            if (damage < 30f)
            {
                //可以添加格挡的判断进行格挡或者闪避，同时扣除耐力值
                
            }
            else
            {
                //不能释放格挡，直接受伤
                animator.Play(hitName);
                GamePoolManager.MainInstance.GetItem("HitSound", transform.position, Quaternion.identity);
            }
        }

        
    }
}
