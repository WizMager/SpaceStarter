using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/PlanetData", fileName = "PlanetData")]
    public class PlanetData : ScriptableObject
    {
        public float gravity;
        public float speedRotationAroundPlanet;
        public float engineForce;
        public float moveSpeedToDirection;
        public float rotationSpeedToDirection;
    }
}