using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/BonusData", fileName = "BonusData")]
    public class BonusData : ScriptableObject
    {
        public int goodBonus;
        public int badBonus;
    }
}