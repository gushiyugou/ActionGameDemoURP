using System;
using System.Collections;
using System.Collections.Generic;
using Action_ARPG.Movement;
using GGG.Tool.Singleton;
using UnityEngine;

public class EnemyManager : Singletons<EnemyManager>
{
    [SerializeField]private Transform mainPlayer;


    protected override void Awake()
    {
        base.Awake();
        //mainPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }


    public Transform GetMainPlayer() => mainPlayer;


    private void OnDisable()
    {
        Debug.Log(0000);
    }
}
