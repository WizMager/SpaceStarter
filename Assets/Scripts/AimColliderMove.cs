using UnityEngine;

namespace DefaultNamespace
{
    public class AimColliderMove : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransfrom;

        private void Update()
        {
            var transformCollider = transform;
            transformCollider.position = _playerTransfrom.position;
        }
    }
}