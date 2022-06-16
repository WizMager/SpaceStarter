using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MaterialData", fileName = "MaterialData")]
    public class MaterialsData : ScriptableObject
    {
        [Header("Trees")]
        public Material[] tree1Type;
        public Material[] tree2Type;
        public Material[] tree3Type;
        public Material[] tree4Type;
        [Header("Houses")] 
        public Material[] glassFloor;
        public Material[] house1Type;
        public Material[] house2Type;
        public Material[] house3Type;
        public Material[] house4Type;
        public Material[] house5Type;
        public Material[] house6Type;
        [Header("Other")] 
        public Material[] atmospherePlanet;
        public Material[] grassPlanet;
        public Material[] groundPlanet;
        public Material[] waterPlanet;
        public Material[] chelik;
    }
}