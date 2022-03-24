using UnityEngine;

namespace Utils
{
    public class AimCollider : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransfrom;

        private void Update()
        {
            if (_playerTransfrom != null) return;
                Destroy(gameObject);
        }
    }
}