using Interface;
using UnityEngine;

namespace Controllers
{
    public class CameraController : IExecute
    {
        private readonly Transform _player;
        private readonly Transform _camera;

        public CameraController(Transform playerTransform, Transform cameraTransform)
        {
            _player = playerTransform;
            _camera = cameraTransform;
        }
        
        private void FollowPlayer()
        {
            var offsetPosition = _player.position;
            offsetPosition.y = _camera.position.y;
            _camera.position = offsetPosition;
        }
        
        public void Execute(float deltaTime)
        {
            FollowPlayer();
        }
    }
}