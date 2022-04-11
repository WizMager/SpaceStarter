using UnityEngine;
using UnityEngine.Jobs;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/CameraData", fileName = "CameraData")]
    public class CameraData : ScriptableObject
    {
        [Header("Camera Up")]
        public float upSpeed;
        public float upOffsetFromPlayer;
        public float moveSpeed;
        [Header("First Person(Last Planet)")]
        public float firstPersonRotationSpeed;
        [Header("Camera Down")]
        public float cameraDownPosition;
        public float cameraDownSpeed;
        [Header("Rotate Around Planet")] 
        public float cameraOffsetBeforeRotation;
        [Header("Fly from planet")]
        public float moveSpeedFlyFromPlanet;
        public float offsetBackFlyFromPlanet;
        public float offsetUpFlyFromPlanet;
        
        [Header("Fly radius")]
        public float rotationSpeedFlyRadius;
        public float moveSpeedFlyRadius;
        public float offsetBackFlyRadius;
        public float offsetUpFlyRadius;
        
        [Header("Fly camera down")]
        public float rotationSpeedFlyCameraDown;
        public float moveSpeedFlyCameraDown;
        public float offsetBackFlyCameraDown;
        public float offsetUpFlyCameraDown;
        
        [Header("Fly first person")]
        public float rotationSpeedFlyFirstPerson;
        public float moveSpeedFlyFirstPerson;
        public float offsetBackFlyFirstPerson;
        public float offsetUpFlyFirstPerson;

        [Header("Fly away (1st stage)")]
        public float timeFlyAway1;
        public float rotationSpeedFlyAway1;
        public float moveSpeedFlyAway1;
        public float offsetBackFlyAway1;
        public float offsetUpFlyAway1;
        
        [Header("Fly away (2nd stage)")]
        public float rotationSpeedFlyAway2;
        public float moveSpeedFlyAway2;
        public float offsetBackFlyAway2;
        public float offsetUpFlyAway2;

    }
}