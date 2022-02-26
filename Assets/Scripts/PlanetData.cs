using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlanetData", fileName = "PlanetData")]
public class PlanetData : ScriptableObject
{
   public float swipeSensitivity;
   public Transform planetCenter;
}