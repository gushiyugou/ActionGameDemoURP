using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Action_ARPG.Movement
{
    public class PlayerMovementControl : CharacterMovementControllerBase
    {
        private float _rotationAngle;
        private float _rotationVelocity = 0f;
        [Header("旋转平滑时间")]
        [SerializeField]private float _smoothDampTime;
        private Transform _mainCamera;

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main.transform;
        }

        protected override void LateUpdate()
        {
            UpdateAnimation();
            CharacterRotationControl();
        }
        

        private void CharacterRotationControl()
        {
            // if(!_characterOnGround) return;
            if (_animator.GetBool("HasInput"))
            {
                _rotationAngle = Mathf.Atan2(GameInputManager.MainInstance.MovementInput.x,
                                     GameInputManager.MainInstance.MovementInput.y) *
                                 Mathf.Rad2Deg+ _mainCamera.eulerAngles.y;
            }

            if (_animator.GetBool("HasInput") && _animator.AnimationAtTag("motion"))
            {
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    _rotationAngle,ref _rotationVelocity,_smoothDampTime);
                
            }
        }
        
        
        #region 动画相关

        private void UpdateAnimation()
        {
            if (!_characterOnGround)
            {
                //根据需要来调整
                // _animator.SetBool("HasInput", false);
                return;
            }
            _animator.SetBool("HasInput",GameInputManager.MainInstance.MovementInput != Vector2.zero);
           
            if (_animator.GetBool("HasInput"))
            {
                if(GameInputManager.MainInstance.Run)
                    _animator.SetBool("Run",true);
                
                _animator.SetFloat("Movement",(_animator.GetBool("Run")?
                        2f: GameInputManager.MainInstance.MovementInput.sqrMagnitude),
                    0.25f,Time.deltaTime);
            }
            else
            {
                _animator.SetFloat("Movement",0f, 0.25f,Time.deltaTime);
                
                if (_animator.GetFloat("Movement") < 0.2f)
                    _animator.SetBool("Run",false);
            }
            
        }

        #endregion
    }
}
