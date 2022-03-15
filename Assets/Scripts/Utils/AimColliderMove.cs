using UnityEngine;

namespace Utils
{
    public class AimColliderMove : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransfrom;

        private void Update()
        {
            if (_playerTransfrom == null)
            {
                Destroy(gameObject);
            }
            else
            {
                var transformCollider = transform;
                transformCollider.position = _playerTransfrom.position; 
            }
        }
    }
}