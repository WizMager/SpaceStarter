using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/PortalData", fileName = "PortalData")]
    public class PortalData : ScriptableObject
    {
        public GameObject portalPrefab;
    }
    
}
