using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/ObjectsOnPlanetData", fileName = "ObjectsOnPlanetData")]
    public class ObjectsOnPlanetData : ScriptableObject
    {
        public ObjectTypeOnPlanet[] objectsTypeOnPlanet;

        [Header("Building Around Planet Generation")]
        public float minimalAngleBetweenBuildings;
        public float maximumAngleBetweenBuildings;
        public int buildingsWithBonus;
    }
}