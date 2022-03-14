﻿using State;

namespace DefaultNamespace
{
    public class LastPlanetShootState : PlayerState
    {
        private CameraController _cameraController;
        private bool _isActivated;

        public LastPlanetShootState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            _cameraController.FirstPersonActivation();
        }
        
        public override void Move(float deltaTime)
        {
            if (_isActivated) return;
            
            if (!_cameraController.CameraStopped()) return;
            
            PlayerController.ShootLastPlanet();
            _isActivated = true;
        }
    }
}