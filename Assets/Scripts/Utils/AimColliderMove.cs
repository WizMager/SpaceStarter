using UnityEngine;

namespace Utils
{
    public class AimColliderMove : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransfrom;
        private Vector3 yOffset = new Vector3(0, -5, 0);

        private void Update()
        {
            if (_playerTransfrom == null)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = _playerTransfrom.position;
                transform.position += yOffset;
            }
        }
    }
}