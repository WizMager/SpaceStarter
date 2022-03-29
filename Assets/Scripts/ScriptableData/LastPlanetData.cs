using UnityEngine;

namespace ScriptableData
{
   [CreateAssetMenu(menuName = "Data/LastPlanetData", fileName = "LastPlanetData")]
   public class LastPlanetData : ScriptableObject
   {
      [Header("Shoot Phase")]
      public float explosionArea;
      public float explosionForce;
      public GameObject explosionParticle;
      [Header("Fly Phase First Person")]
      public float distanceFromLastPlanetToStop;
      public float moveSpeedFirstPerson;
      public int minimalPercentMoveSpeedFirstPerson;
      [Header("Fly Phase 3D")]
      public float moveSpeedFromAbove;
      public float cameraDownSpeed;
      public float cameraDownPosition;
   }
}