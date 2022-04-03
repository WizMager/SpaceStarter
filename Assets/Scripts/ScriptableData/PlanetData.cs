using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/PlanetData", fileName = "PlanetData")]
    public class PlanetData : ScriptableObject
    {
        [Header("Fly Around Planet")] 
        public float flyAngle;
        public float startGravity;
        public float gravityAcceleration;
        public float maxGravity;
        public float startSpeedRotationAroundPlanet;
        public float startEngineForce;
        public float engineAcceleration;
        public float maxEngineForce;
        [Header("Edge Gravity From Planet")]
        public float moveSpeedToEdgeGravity;
        public float rotationTimeToEdgeGravity;
        [Header("Fly To Center Gravity")]
        public float moveSpeedCenterGravity;
        public float rotationInGravitySpeed;
        [Header("Edge Gravity To Planet")] 
        public float moveSpeedToPlanet;
        [Header("Look To Planet")] 
        public float rotationSpeedLookPlanet;
    }
}