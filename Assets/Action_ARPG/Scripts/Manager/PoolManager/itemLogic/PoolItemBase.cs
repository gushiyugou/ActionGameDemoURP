using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooltem
{
    void Spawn();
    void Recycl();
}
public class PoolItemBase : MonoBehaviour,IPooltem
{
    private void OnEnable()
    {
        Spawn();
    }

    private void OnDisable()
    {
        Recycl();
    }

    public virtual void Spawn()
    {
        
    }

    public virtual void Recycl()
    {
        
    }
}
