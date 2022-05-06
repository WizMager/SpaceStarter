using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/PrefabLinkData", fileName = "PrefabLinkData")]
    public class PrefabLinkData : ScriptableObject
    {
        public GameObject missilePrefab;
    }
}