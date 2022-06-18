using System.Collections.Generic;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MaterialType/AtmosphereMaterial", fileName = "AtmosphereMaterialType")]
    public class AtmosphereTypeMaterial : ScriptableObject
    {
        public List<Material> materials;
    }
}