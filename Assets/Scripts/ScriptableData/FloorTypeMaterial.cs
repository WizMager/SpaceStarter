using System.Collections.Generic;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MaterialType/FloorMaterial", fileName = "FloorMaterialType")]
    public class FloorTypeMaterial : ScriptableObject
    {
        public List<Material> materials;
    }
}