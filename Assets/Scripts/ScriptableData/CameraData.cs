using UnityEngine;

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
    }
}