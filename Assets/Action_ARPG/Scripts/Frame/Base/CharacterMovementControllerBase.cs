using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


namespace Action_ARPG
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class CharacterMovementControllerBase : MonoBehaviour
    {
        //角色基础属性
        protected CharacterController _characterController;
        protected Animator _animator;

        protected Vector3 _movementDirection;

        #region ****************地面检测相关参数****************
        
        [Header("地面检测相关参数")]
        protected bool _characterOnGround;
        [SerializeField]protected float _detectionGroundPositionOffset;
        [SerializeField]protected float _detectionGroundRange;
        [SerializeField]protected LayerMask _whatIsGround;
        
        #endregion

        #region ****************重力应用相关参数****************
        
        protected readonly float _gravity = -9.8f;
        protected float _characterVerticalVelocity;
        protected float _characterVerticalMaxVelocity = 54f;
        protected float _fallOutDeltaTime;
        protected float _fallOutTime = 0.05f;
        protected Vector3 _characterVelocityDerection;
        protected bool _isEnabelGravity;

        #endregion


        #region ****************生命周期函数、引擎回调函数相关****************

        protected virtual void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            _fallOutDeltaTime = _fallOutTime;
            _isEnabelGravity = true;
        }

        protected virtual void OnEnable()
        {
            GameEventManager.MainInstance.AddEventListening<bool>("EnableCharacterGravity",EnableCharacterGravity);
        }

        protected virtual void OnDisable()
        {
            GameEventManager.MainInstance.RemoveEventListening<bool>("EnableCharacterGravity",EnableCharacterGravity);
            
        }

        protected virtual void Update()
        {
            
            CharacterApplyGravity();
            UpdateCharacterGravity();
            
        }

        protected virtual void LateUpdate()
        {
            
        }

        protected virtual void OnAnimatorMove()
        {
            _animator.ApplyBuiltinRootMotion();
            CharacterMovementDirction(_animator.deltaPosition);
        }

        #endregion
        

        #region ****************地面检测函数****************

        protected bool GroundDetection()
        {
            Vector3 detectionPosition = new Vector3(transform.position.x,
                transform.position.y - _detectionGroundPositionOffset,transform.position.z);
            return Physics.CheckSphere(detectionPosition, _detectionGroundRange, _whatIsGround, QueryTriggerInteraction.Ignore);
        }

        #endregion


        #region  ****************重力应用函数****************

        protected void CharacterApplyGravity()
        {
            _characterOnGround = GroundDetection();
            if (_characterOnGround)
            {
                _fallOutDeltaTime = _fallOutTime;
                if (_characterVerticalVelocity < 0)
                    _characterVerticalVelocity = -2f;
                else
                {
                    _characterVerticalVelocity += _gravity * Time.deltaTime;
                }
            }
            else
            {
                if (_fallOutDeltaTime > 0)
                    _fallOutDeltaTime -= Time.deltaTime;
                else
                {
                    //可以执行一些动画播放的效果
                }

                if (_characterVerticalVelocity < _characterVerticalMaxVelocity && _isEnabelGravity)
                    _characterVerticalVelocity += _gravity * Time.deltaTime;
                else
                    _characterVerticalVelocity = _characterVerticalMaxVelocity;
            }
        }

        private void UpdateCharacterGravity()
        {
            if(!_isEnabelGravity) return;   
            _characterVelocityDerection.Set(0,_characterVerticalVelocity,0);
            _characterController.Move(_characterVelocityDerection*Time.deltaTime);
        }
        
        //坡道检测
        protected Vector3 SlopResetDirction(Vector3 moveDerection)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, out var hitInfo,
                    _characterController.height * 0.85f))
            {
                if (Vector3.Dot(Vector3.up, hitInfo.normal) != 0)
                {
                    return moveDerection = Vector3.ProjectOnPlane(moveDerection,hitInfo.normal);
                }
            }
            return moveDerection;
        }

        #endregion

        #region  ****************角色移动方向计算****************

        protected void CharacterMovementDirction(Vector3 dirction)
        {
            _movementDirection = SlopResetDirction(dirction);
            _characterController.Move(_movementDirection);
        }

        #endregion

       #region ****************注册事件相关****************

       private void ChangeCharacterVerticalVelocity(float targetVelocity)
       {
           DevelopmentToos.WTF("未改变之前的速度："+_characterVerticalVelocity);
           _characterVerticalVelocity = targetVelocity;
           DevelopmentToos.WTF("改变之后的速度："+_characterVerticalVelocity);
       }

       protected void EnableCharacterGravity(bool enable)
       {
           
           _isEnabelGravity = enable;
           _characterVerticalVelocity = enable ? -9.8f : 0;
           // float countTime = 1.5f;
           // while (true)
           // {
           //     if (countTime <= 0)
           //     {
           //         _characterVerticalVelocity = -9.8f;
           //         return;
           //     }
           //     else
           //         countTime -= Time.deltaTime;
           // }
       }
       #endregion


        #region ****************可视化相关****************

        private void OnDrawGizmos()
        {
            Vector3 detectionPosition = new Vector3(transform.position.x,
                transform.position.y - _detectionGroundPositionOffset,transform.position.z);
            Gizmos.color = _characterOnGround? Color.red : Color.green;
            Gizmos.DrawWireSphere(detectionPosition, _detectionGroundRange);
        }

        #endregion
    }
}

