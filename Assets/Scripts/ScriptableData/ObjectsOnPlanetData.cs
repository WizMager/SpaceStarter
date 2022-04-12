using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/ObjectsOnPlanetData", fileName = "ObjectsOnPlanetData")]
    public class ObjectsOnPlanetData : ScriptableObject
    {
        public ObjectTypeOnPlanet[] objectsTypeOnPlanet;
    }
}