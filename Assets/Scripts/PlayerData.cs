using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerData", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
     public int missileCount;
     public int explosionPower;
}