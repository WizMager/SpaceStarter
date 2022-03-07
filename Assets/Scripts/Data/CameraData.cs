using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/CameraData", fileName = "CameraData")]
    public class CameraData : ScriptableObject
    {
        public float startUpDivision;
        public float upSpeed;
        public float upOffsetFromPlayer;
        public float firstPersonRotationSpeed;
    }
}