using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMachSMB : StateMachineBehaviour
{
    [SerializeField, Header("匹配信息")] private float _startTime;
    [SerializeField] private float _endTime;
    [SerializeField] private AvatarTarget _avatarTarget;

    [SerializeField, Header("是否启用重力")] private bool _isEnableGravity;
    [SerializeField] private float _enableGravityTime;

    private Vector3 _matchPostion;
    private Quaternion _matchRotation;


    private void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListening<Vector3,Quaternion>("SetAnimationMatchInfo",GetMatchInfo);
    }

    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEventListening<Vector3,Quaternion>("SetAnimationMatchInfo",GetMatchInfo);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        // 检查是否处于过渡状态
        if (animator.IsInTransition(layerIndex)) return;
 
        if (!animator.isMatchingTarget)
        {
            animator.MatchTarget(_matchPostion,_matchRotation,_avatarTarget,
                new MatchTargetWeightMask(Vector3.one,0),_startTime,_endTime);
        }

        if (_isEnableGravity)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _endTime)
            {
                //TODO:激活重力
                GameEventManager.MainInstance.CallEvent("EnableCharacterGravity",true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}


    private void GetMatchInfo(Vector3 position, Quaternion rotation)
    {
        _matchPostion = position;
        _matchRotation = rotation;
    }
}
