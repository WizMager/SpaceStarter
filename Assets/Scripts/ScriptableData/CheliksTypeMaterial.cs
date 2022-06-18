using System.Collections.Generic;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MaterialType/ChelikMaterial", fileName = "ChelikMaterialType")]
    public class CheliksTypeMaterial : ScriptableObject
    {
        public List<Material> materials;
    }
}