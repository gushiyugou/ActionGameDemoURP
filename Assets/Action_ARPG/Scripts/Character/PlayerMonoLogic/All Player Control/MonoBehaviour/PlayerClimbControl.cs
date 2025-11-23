using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbControl : MonoBehaviour
{
    private Animator _animator;
    
    [SerializeField] private float _detectDistance;
    [SerializeField] private LayerMask _climbLayerMask;

    private RaycastHit hitInfo;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CharacterClimbInput();
    }

    private bool IsCanClimb()
    {
        return Physics.Raycast(transform.position + (transform.up * 0.5f),
            transform.forward, out hitInfo, _detectDistance,
            _climbLayerMask,QueryTriggerInteraction.Ignore);
    }

    private void CharacterClimbInput()
    {
        if(!IsCanClimb()) return;

        if (GameInputManager.MainInstance.Climb)
        {
            float differentialDistance = hitInfo.collider.transform.position.y - transform.position.y;
            float actualDistance = differentialDistance + hitInfo.collider.bounds.extents.y;
            Vector3 position = Vector3.zero;
            var rotation = Quaternion.LookRotation(-hitInfo.normal);
            
            position.Set(hitInfo.point.x,actualDistance,hitInfo.point.z);

            switch (hitInfo.collider.tag)
            {
                case "HighWall":
                    ToCallEvent(position, rotation);
                    _animator.CrossFade("ClimbHighWall",normalizedTransitionDuration:0f,0,0f);
                    break;
                case "MediumWall":
                    ToCallEvent(position, rotation);
                    break;
                case "LowWall":
                    ToCallEvent(position, rotation);
                    break;
            }
        }
    }

    private void ToCallEvent(Vector3 position, Quaternion rotation)
    {
        GameEventManager.MainInstance.CallEvent<Vector3,Quaternion>("SetAnimationMatchInfo",position,rotation);
        GameEventManager.MainInstance.CallEvent<bool>("EnableCharacterGravity",false);
    }
}
