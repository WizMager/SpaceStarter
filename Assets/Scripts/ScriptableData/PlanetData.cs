using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/PlanetData", fileName = "PlanetData")]
    public class PlanetData : ScriptableObject
    {
        public float gravity;
        public float speedRotationAroundPlanet;
        public float engineForce;
        public float moveSpeedToDirection;
        public float rotationSpeedToDirection;
        public float moveSpeedCenterGravity;
        public float cameraDownOffset;
    }
}