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
      public float distanceFromLastPlanetToStop;
      public float moveSpeedToLastPlanet;
      [Header("Fly Phase")]
      public Vector3 center;
      public float moveSpeedToPlanet;
      public float cameraDownSpeed;
      public float cameraDownPosition;
   }
}