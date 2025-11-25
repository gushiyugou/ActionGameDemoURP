using System.Collections;
using System.Collections.Generic;
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
            animator.Play(hitName);
            //获取音效
            GamePoolManager.MainInstance.GetItem("HitSound", transform.position, Quaternion.identity);
        }


        
        

        
    }
}
