using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlanetData", fileName = "PlanetData")]
public class PlanetData : ScriptableObject
{
   public GameObject explosionParticle;
   public Transform planetCenter;
}