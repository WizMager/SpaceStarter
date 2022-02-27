using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlanetData", fileName = "PlanetData")]
public class PlanetData : ScriptableObject
{
       public float gravity;
       public float speedRotation;
       public float engineForce;
}