using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/PlanetData", fileName = "PlanetData")]
    public class PlanetData : ScriptableObject
    {
        [Header("Fly Around Planet")]
        public float gravity;
        public float speedRotationAroundPlanet;
        public float engineForce;
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