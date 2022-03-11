using UnityEngine;

namespace DefaultNamespace
{
    public class AimColliderMove : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransfrom;

        private void Update()
        {
            transform.position = _playerTransfrom.position;
        }
    }
}