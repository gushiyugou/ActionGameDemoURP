using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // GameEventManager.MainInstance.CallEvent<float>("改变角色垂直速度",10f);
            GameObject obj = GamePoolManager.MainInstance.GetItem("Bullet");
        }
    }
}
