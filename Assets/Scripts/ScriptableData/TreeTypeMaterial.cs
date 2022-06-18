using System.Collections.Generic;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MaterialType/TreeMaterial", fileName = "TreeMaterialType")]
    public class TreeTypeMaterial : ScriptableObject
    {
        public List<Material> materials;
    }
}