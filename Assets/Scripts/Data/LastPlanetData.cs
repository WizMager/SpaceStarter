using UnityEngine;

[CreateAssetMenu(menuName = "Data/LastPlanetData", fileName = "LastPlanetData")]
public class LastPlanetData : ScriptableObject
{
   public GameObject explosionParticle;
   public GameObject explosionBox;
   public float explosionBoxForce;
   public GameObject center;
}