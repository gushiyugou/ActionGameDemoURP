using System.Collections;
using System.Collections.Generic;
using Action_ARPG.Health;
using GGG.Tool;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "new CHaracterHealthInfo",menuName = "Character/Health/CharacterHealthInfo")]
public class CharacterHealthInfo_SO : ScriptableObject
{
    //1.角色最大生命值
    //2.角色最大能量值
    //3.角色当前生命值
    //4.角色当前能量值
    //5.角色是否死亡
    //6,角色体力值是否充沛
    [SerializeField] private CharacterHealthBaseData_SO HealthBaseDataSO;
    [SerializeField] private float maxHP;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float currentHP;
    [SerializeField] private float currentEnergy;
    [SerializeField] private bool isDead => (currentHP<= 0);
    [SerializeField] private bool isEnergyFull;

    public float MaxHP => maxHP;
    public float MaxEnergy => maxEnergy;
    public float CurrentHP => currentEnergy;
    public float CurrentEnergy => currentEnergy;
    public bool IsDaed => isDead;
    public bool IsEnergyFull => isEnergyFull;


    public void InitCharacterHealthInfo()
    {
        if (HealthBaseDataSO == null)
        {
            DevelopmentToos.WTF($"{this.GameObject().name}没有配置HealthBaseDataSO数据，配置后初始化");
            return;
        }
        maxHP = HealthBaseDataSO.MaxHP;
        maxEnergy = HealthBaseDataSO.MaxEnergy;
        currentHP = HealthBaseDataSO.MaxHP;
        currentEnergy = HealthBaseDataSO.MaxEnergy;
        isEnergyFull = true;
    }

    public void DamageToHP(float damage)
    {
        //判断当前敌人是否正在攻击动画状态中
        //判断当前敌人能量值是否满，满的则需先扣除消耗的能量值
        if (isEnergyFull)
        {
            currentEnergy = Clamp(currentEnergy, damage, 0f, maxEnergy);
            if(currentEnergy <= 0f)
            {
                isEnergyFull = false;
            }
        }
        currentHP = Clamp(currentHP, damage, 0f, maxHP);
    }
    
    public void RecoverHP(float recoverValue)
    {
        currentHP = Clamp(currentHP, recoverValue, 0f, maxHP,true);
    }
    
    public void RecoverEnergy(float recoverValue)
    {
        currentHP = Clamp(currentHP, recoverValue, 0f, maxEnergy,true);
        if (currentEnergy == maxEnergy) isEnergyFull = true;
    }
    
    public void DamageToEnergy(float damage)
    {
        if (isEnergyFull)
        {
            currentEnergy = Clamp(currentEnergy, damage, 0f, maxEnergy);
        }
        if(currentEnergy <= 0f)
        {
            isEnergyFull = false;
        }
    }
    
    
    private float Clamp(float value,float offsetValue,float minValue,float maxValue,bool isAdd = false)
    {
        return Mathf.Clamp((isAdd) ? value + offsetValue : value - offsetValue, minValue, maxValue);
    }
}
