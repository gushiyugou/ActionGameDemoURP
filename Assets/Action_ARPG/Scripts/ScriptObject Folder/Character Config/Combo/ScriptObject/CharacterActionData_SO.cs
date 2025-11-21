using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Action_ARPG.ComboData
{
    [CreateAssetMenu(fileName = "new Action",menuName = "Character/Action")]
    public class CharacterActionData_SO : ScriptableObject
    {
        [SerializeField] private string animationName;
        [SerializeField] private string[] comboHitName;
        [SerializeField] private string[] comboParryName;
        [SerializeField] private float comboDamage;
        [SerializeField] private float comboColdTime;
        [SerializeField] private float comboPositionOffset;
        
        
        public string AnimationName => animationName;
        public string[] ComboHitName => comboHitName;
        public string[] ComboParryName => comboParryName;
        public float ComboDamage => comboDamage;
        public float ComboColdTime => comboColdTime;
        public float ComboPositionOffset => comboPositionOffset;

        /// <summary>
        /// 获取当前连招击中伤害的最大数量
        /// </summary>
        /// <returns></returns>
        public int GetComboHitMaxCount() => comboHitName.Length;
        public int GetComboParryMaxCount() => comboParryName.Length;
    }
}
