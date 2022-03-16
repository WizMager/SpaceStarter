using UnityEngine;

namespace ScriptableData
{
     [CreateAssetMenu(menuName = "Data/PlayerData", fileName = "PlayerData")]
     public class PlayerData : ScriptableObject
     {
          public int missileCount;
          public int startHealth;
     }
}