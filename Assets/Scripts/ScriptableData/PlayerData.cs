using UnityEngine;

namespace ScriptableData
{
     [CreateAssetMenu(menuName = "Data/PlayerData", fileName = "PlayerData")]
     public class PlayerData : ScriptableObject
     {
          public int missileCount;
          public int startHealth;
          public float cooldownTakeDamage;
          public float multiplyDamageTake;
          public float startDamageTake;
          public float endDamageTake;
          public float thresholdAfterTouchPlanetGravity;
     }
}