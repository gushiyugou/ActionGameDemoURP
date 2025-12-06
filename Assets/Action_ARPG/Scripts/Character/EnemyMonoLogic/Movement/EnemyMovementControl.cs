using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;


namespace Action_ARPG.Movement
{
    public class EnemyMovementControl : CharacterMovementControllerBase
    {
        //1.动画控制
        //2.播放移动动画时，应该让AI看向玩家方向
        private bool applyMovement;

        protected override void Start()
        {
            base.Start();
            SetApplyMovement(true);
        }

        protected override void Update()
        {
            base.Update();
            LookMainPlayer();
            DrawDistance();
        }





        private void LookMainPlayer()
        {
            if (EnemyManager.MainInstance.GetMainPlayer() != null)
            {
             
                transform.Look(EnemyManager.MainInstance.GetMainPlayer().position,500f);
                // Vector3 direction = (EnemyManager.MainInstance.GetMainPlayer().position - transform.position)
                //     .normalized;
                
                // transform.forward = Vector3.Slerp(transform.forward,EnemyManager.MainInstance.GetMainPlayer().position-transform.position,0.1f);
            }
        }


        /// <summary>
        /// 设置东环状态机中控制AI的速度的值
        /// </summary>
        /// <param name="horizontal">水平方向速度控制变量</param>
        /// <param name="vertical">数值方向速度控制变量</param>
        public void SetAnimatorMovementValue(float horizontal, float vertical)
        {
            if (applyMovement)
            {
                _animator.SetBool(AnimationID.HasInputID,true);
                _animator.SetFloat(AnimationID.LockID,1f);
                _animator.SetFloat(AnimationID.HorizontalID,horizontal,0.2f,Time.deltaTime);
                _animator.SetFloat(AnimationID.VerticalID,vertical,0.2f,Time.deltaTime);
            }
            else
            {
                _animator.SetBool(AnimationID.HasInputID,false);
                _animator.SetFloat(AnimationID.LockID,0);
                _animator.SetFloat(AnimationID.HorizontalID,0,0.2f,Time.deltaTime);
                _animator.SetFloat(AnimationID.VerticalID,0,0.2f,Time.deltaTime);
            }
        }

        private void DrawDistance()
        {
            if (EnemyManager.MainInstance.GetMainPlayer() == null) return;
            Debug.DrawRay(transform.position+(transform.up*0.7f),
                EnemyManager.MainInstance.GetMainPlayer().position-transform.position,Color.red);
        }

        public void SetApplyMovement(bool apply)
        {
            applyMovement = apply;
        }
    }
}

