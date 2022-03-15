using UnityEngine;

namespace Utils
{
    public class ParticleDestroyer : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 1f);
        }
    }
}