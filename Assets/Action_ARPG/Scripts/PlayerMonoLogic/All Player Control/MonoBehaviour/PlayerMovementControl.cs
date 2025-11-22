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
        
        //脚步声
        private float _nextFootTime;
        [SerializeField] private float _slowFootTime;
        [SerializeField] private float _fastFootTime;
        
        //角色的目标朝向
        private Vector3 _characterTargetDirectiuon;

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
            if (_animator.GetBool(AnimationID.HasInputID))
            {
                _rotationAngle = Mathf.Atan2(GameInputManager.MainInstance.MovementInput.x,
                                     GameInputManager.MainInstance.MovementInput.y) *
                                 Mathf.Rad2Deg+ _mainCamera.eulerAngles.y;
            }

            if (_animator.GetBool(AnimationID.HasInputID) && _animator.AnimationAtTag("motion"))
            {
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    _rotationAngle,ref _rotationVelocity,_smoothDampTime);
                _characterTargetDirectiuon = Quaternion.Euler(0, _rotationAngle, 0) * Vector3.forward;
            }
            _animator.SetFloat(AnimationID.DeltaAngleID,DevelopmentToos.GetDeltaAngle(transform,_characterTargetDirectiuon.normalized));
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
            _animator.SetBool(AnimationID.HasInputID,GameInputManager.MainInstance.MovementInput != Vector2.zero);
           
            if (_animator.GetBool(AnimationID.HasInputID))
            {
                if(GameInputManager.MainInstance.Run)
                    _animator.SetBool(AnimationID.RunID,true);
                
                _animator.SetFloat(AnimationID.MovementID,(_animator.GetBool(AnimationID.RunID)?
                        2f: GameInputManager.MainInstance.MovementInput.sqrMagnitude),
                    0.25f,Time.deltaTime);
                SetCharacterFootSound();
            }
            else
            {
                _animator.SetFloat(AnimationID.MovementID,0f, 0.25f,Time.deltaTime);
                
                if (_animator.GetFloat(AnimationID.MovementID) < 0.2f)
                    _animator.SetBool(AnimationID.RunID,false);
            }
            
        }

        public void SetCharacterFootSound()
        {
            if (_characterOnGround && _animator.GetFloat(AnimationID.MovementID) > 0.5f &&
                _animator.AnimationAtTag("motion"))
            {
                _nextFootTime-= Time.deltaTime;
                if(_nextFootTime < 0) PlayFootStepSound();
            }
            else  
                _nextFootTime = 0f;
        }

        private void PlayFootStepSound()
        {
            GamePoolManager.MainInstance.GetItem("FootStepSound", transform.position, Quaternion.identity);
            _nextFootTime = _animator.GetFloat(AnimationID.MovementID)>1.1f?_fastFootTime:_slowFootTime;
        }
        #endregion
    }
}
