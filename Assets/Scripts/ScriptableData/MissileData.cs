using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MissleData", fileName = "MissleData")]
    public class MissileData : ScriptableObject
    {
        public GameObject missile;
        [Header("Missile engine")]
        public float timeBeforeStartEngine;
        public float timeBeforeEngineStop;
        public float initialImpulse;
        public float engineAcceleration;
        public float rotationSpeed;
        [Header("Explosion")]
        public GameObject explosionParticleSystem;
        public float explosionArea;
        public float explosionForce;
        public float explosionDelay;
    }
    
}
