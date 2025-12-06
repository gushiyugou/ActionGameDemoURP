using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CharacterHealthBaseData",menuName = "Character/Health/CharacterHealthBaseData")]
public class CharacterHealthBaseData_SO : ScriptableObject
{
    //1.处理每一种敌人的基础血量和能量值
    //2.由于敌人种类存在差异性，其最大生命值和能量值也有所不同，同样的角色与角色之间的基础属性也会存在差异
    [SerializeField] private float maxHP;
    [SerializeField] private float maxEnergy;


    public float MaxHP => maxHP;
    public float MaxEnergy => maxEnergy;
}
