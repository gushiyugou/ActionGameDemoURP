using System.Collections.Generic;
using UnityEngine;

namespace Action_ARPG.ComboData
{
    [CreateAssetMenu(fileName = "New Combo", menuName = "Character/Combo")]
    public class CharacterCombo_SO : ScriptableObject
    {
        [SerializeField] private List<CharacterActionData_SO> _comboList = new List<CharacterActionData_SO>();
        
        /// <summary>
        /// 获取连招列表中的指定招式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetOneComboAction(int index)
        {
            if (_comboList.Count == 0 || index > _comboList.Count || index < 0) return null;
            return _comboList[index].AnimationName;
        }

        /// <summary>
        /// 获取对应的受伤动画
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hitIndex"></param>
        /// <returns></returns>
        public string GetHitName(int index, int hitIndex)
        {
            if (_comboList.Count == 0 || index > _comboList.Count || index < 0) return null;
            if (_comboList[index].GetComboHitMaxCount() == 0 || hitIndex > _comboList[index].GetComboHitMaxCount() ||
                hitIndex < 0) return null;
            
            return _comboList[index].ComboHitName[hitIndex];
        }
        
        /// <summary>
        /// 获取对应的格挡动画
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hitIndex"></param>
        /// <returns></returns>
        public string GetParryName(int index, int hitIndex)
        {
            if (_comboList.Count == 0 || index > _comboList.Count || index < 0) return null;
            if (_comboList[index].GetComboParryMaxCount() == 0 || hitIndex > _comboList[index].GetComboParryMaxCount() ||
                hitIndex < 0) return null;
            
            return _comboList[index].ComboParryName[hitIndex];
        }

        public float GetComboDamage(int index)
        {
            if (_comboList.Count == 0 || index > _comboList.Count || index < 0) return 0f;

            return _comboList[index].ComboDamage;
        }


        public float GetComboColdTime(int index)
        {
            if (_comboList.Count == 0 || index > _comboList.Count || index < 0) return 0f;

            return _comboList[index].ComboColdTime;
        }
        
        public int GetComboHitMaxCount(int index)=> _comboList[index].GetComboHitMaxCount();
        public int GetComboParryMaxCount(int index) => _comboList[index].GetComboParryMaxCount();
        public int GetComboMaxCount() => _comboList.Count;
    }
    
    
}