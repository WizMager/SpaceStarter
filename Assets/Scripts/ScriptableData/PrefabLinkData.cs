using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/PrefabLinkData", fileName = "PrefabLinkData")]
    public class PrefabLinkData : ScriptableObject
    {
        public GameObject missilePrefab;
        public GameObject[] trees;
        public GameObject[] cheliks;
        public GameObject[] _floors;
        public GameObject[] _roofs;
    }
}