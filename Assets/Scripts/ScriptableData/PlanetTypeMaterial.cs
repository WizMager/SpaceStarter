using System.Collections.Generic;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MaterialType/PlanetMaterial", fileName = "PlanetMaterialType")]
    public class PlanetTypeMaterial: ScriptableObject
    {
        public List<Material> materials;
    }
}