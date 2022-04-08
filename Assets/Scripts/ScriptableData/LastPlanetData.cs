using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/LastPlanetData", fileName = "LastPlanetData")]
    public class LastPlanetData : ScriptableObject
    {
        [Header("Fly Phase First Person")]
        public float speedDreft;
        public float distanceFromLastPlanetToStop;
        public float moveSpeedFirstPerson;
        public int minimalPercentMoveSpeedFirstPerson;
        [Header("Fly Phase 3D")]
        public float moveSpeedFromAbove;
        public float cameraDownSpeed;
        public float cameraDownPosition;
   }
}