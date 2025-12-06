using System.Collections;
using System.Collections.Generic;
using Action_ARPG.Movement;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AIFreeMovementAction : Action
{
    //1.在没有被分配攻击指令时处于闲置自由移动
    //2.移动的方向随机
    //3.或者播放某些动画
    //4.当玩家距离过近时，应该后退
    private EnemyMovementControl enemyMovementControl;
    private EnemyCommandControl enemyCommandControl;
    private int aiActionIndex;
    private int lastActionindex;
    private float actionTime;


    public override void OnAwake()
    {
        enemyMovementControl = GetComponent<EnemyMovementControl>();
        enemyCommandControl = GetComponent<EnemyCommandControl>();
        lastActionindex = aiActionIndex;
        actionTime = 1f;
    }

    public override TaskStatus OnUpdate()
    {
        if (!enemyCommandControl.GetAttackCommand())
        {
            //处于当前节点的逻辑
            if (PlayerDistance() < 3f)
            {
                enemyMovementControl.SetAnimatorMovementValue(0,1f);
            }
            else if(PlayerDistance() <8.0f+0.1f && PlayerDistance() > 3.0f+0.1f)
            {
                FreeMovement();
                UpdateFreeAction();
                 
            }
            else
            {
                enemyMovementControl.SetAnimatorMovementValue(0,-1f);
            }
           
            return TaskStatus.Running;
        }
        else 
        {
             

        }
        
        return TaskStatus.Success;


        
    }



    private float PlayerDistance()
    {
        if (EnemyManager.MainInstance.GetMainPlayer() == null) return 0f;
        return Vector3.Distance(EnemyManager.MainInstance.GetMainPlayer().position, transform.position);
    }

    private void FreeMovement()
    {
        Debug.Log(aiActionIndex);
        switch (aiActionIndex)
        {
            //0:往左，1：往右
            case 0:
                enemyMovementControl.SetAnimatorMovementValue(-1f,0f);
                break;
            case 1:
                enemyMovementControl.SetAnimatorMovementValue(1f,0f);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
        }
    }


    private void UpdateFreeAction()
    {
        if (actionTime > 0)
        {
            actionTime -= Time.deltaTime;
            if (actionTime <= 0f)
            {
                UpdateActionIndex();
            }
        }
    }

    private void UpdateActionIndex()
    {
        lastActionindex = aiActionIndex;
        aiActionIndex = Random.Range(0, 2);
        actionTime = 2f;
        if (aiActionIndex == lastActionindex)
        {
            aiActionIndex = Random.Range(0, 2);
        }
    }
    
    //1.AI启动，先检测攻击指定
    //2.攻击指令没有被激活，则进行距离判断
    //3.当抵达安全距离，攻击指令还没有被激活
    //4.则进行自由动作的播放
    //5.当攻击指令被激活，则退出节点
    
}
