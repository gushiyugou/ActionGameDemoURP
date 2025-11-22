using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action_ARPG.Event
{
    public class AnimationEvent : MonoBehaviour
    {
        private PlayerComboCotrol playerComboCotrol;

        private void Awake()
        {
            playerComboCotrol = GetComponentInParent<PlayerComboCotrol>();
        }

        private void PlaySound(string name)
        {
            GamePoolManager.MainInstance.GetItem(name,transform.position,Quaternion.identity);
        }


        private void CanChange()
        {
            playerComboCotrol.canChangeState = true;
        }
    }
}
