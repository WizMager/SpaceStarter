
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/BuildingsData", fileName = "BuildingsData")]
    public class BuildingsData : ScriptableObject
    {
        public GameObject building1Prefab;
        public int Building1Count;
        public GameObject building2Prefab;
        public int Building2Count;
        
    }
}