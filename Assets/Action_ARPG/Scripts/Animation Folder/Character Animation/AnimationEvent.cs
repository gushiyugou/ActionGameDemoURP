using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action_ARPG.Event
{
    public class AnimationEvent : MonoBehaviour
    {
        private PlayerComboControl _playerComboControl;

        private void Awake()
        {
            _playerComboControl = GetComponentInParent<PlayerComboControl>();
        }

        private void PlaySound(string name)
        {
            GamePoolManager.MainInstance.GetItem(name,transform.position,Quaternion.identity);
        }


        private void CanChange()
        {
            _playerComboControl.canChangeState = true;
        }
    }
}
