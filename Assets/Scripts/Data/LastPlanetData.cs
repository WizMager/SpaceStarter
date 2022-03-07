using UnityEngine;

namespace Data
{
   [CreateAssetMenu(menuName = "Data/LastPlanetData", fileName = "LastPlanetData")]
   public class LastPlanetData : ScriptableObject
   {
      public float explosionArea;
      public float explosionForce;
      public GameObject explosionParticle;
      public Vector3 center;
   }
}