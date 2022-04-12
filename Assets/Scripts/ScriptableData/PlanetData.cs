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
        public float distanceFromCenterPlanetToSpawn;
        [Header("Arc From Planet (Yellow)")] 
        public float rotationSpeedArcFromPlanet;
        public float moveSpeedArcFromPlanet;
        public float radiusArc;
        public float distanceToCenterRadiusArc;
        public float rotationSpeedRadius;
        [Header("Arc Other")] 
        public float stopDistanceFromPlanetSurface;
        [Range(1, 99)] public int percentOfCameraDownPath;
        public float moveSpeedArcCameraDown;
        public float moveSpeedArcFirstPerson;
        public float timeToDriftAgain;
        [Header("Fly Away")]
        public float waitBeforeFlyAway;
        public float moveSpeedFlyAway;
        public float rotationSpeedFlyAway;
        public float distanceFlyAway;
    }
}