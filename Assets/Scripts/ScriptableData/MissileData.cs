using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/MissileData", fileName = "MissileData")]
    public class MissileData : ScriptableObject
    {
        public GameObject missilePrefab;
        [Header("Missile engine")]
        public float timeBeforeStartEngine;
        public float timeBeforeEngineStop;
        public float initialImpulse;
        public float engineAcceleration;
        public float rotationSpeed;
        [Header("Explosion")]
        public GameObject explosionParticleSystem;
        public float explosionArea;
        public float explosionAreaExt;
        public float explosionForce;
        public float explosionDelay;
        public float scaleModifier;
    }
    
}
