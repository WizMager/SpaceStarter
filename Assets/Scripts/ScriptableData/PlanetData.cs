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
        [Header("Fly To Edge Gravity")]
        public float moveSpeedToEdgeGravity;
        public float rotationSpeedToEdgeGravity;
        [Header("Fly To Center Gravity")]
        public float moveSpeedCenterGravity;
        public float rotationInGravitySpeed;
        [Header("Fly To Next Planet")] 
        public float moveSpeedToNextPlanet;
        [Header("Aim To Asteroid Belt")] 
        public int iterationsCount;
        public float oneStepTimeIteration;
    }
}