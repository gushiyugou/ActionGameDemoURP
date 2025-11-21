using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action_ARPG.Event
{
    public class AnimationEvent : MonoBehaviour
    {
        private void PlaySound(string name)
        {
            GamePoolManager.MainInstance.GetItem(name,transform.position,Quaternion.identity);
        }
    }
}
