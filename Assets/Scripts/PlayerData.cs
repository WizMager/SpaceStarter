using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerData", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
     public int missileCount;
     
     public float explosionArea;
     public float explosionForce;
     public float swipeSensitivity;
     
     public float gravity;
     public float speedRotation;
     public float engineForce;
}