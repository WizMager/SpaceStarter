using System.Collections.Generic;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MaterialData", fileName = "MaterialData")]
    public class MaterialsData : ScriptableObject
    {
        public List<TreeTypeMaterial> tree;
        public List<FloorTypeMaterial> floor;
        public List<AtmosphereTypeMaterial> atmosphere;
        public List<PlanetTypeMaterial> planet;
        public List<CheliksTypeMaterial> chelik;
    }
}