﻿using DefaultNamespace;
using UnityEngine;

namespace State
{
    public class FlyToEdgeGravityPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        
        public FlyToEdgeGravityPlayerState(Vector3 lookDirection, PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            PlayerController.SetDirectionToEdge(lookDirection);
        }

        public override void Move(float deltaTime)
        {
            _cameraController.CameraUp(deltaTime);
            if (!PlayerController.FlyToEdgeGravity()) return;
            PlayerController.TransitionTo(new AimNextPlanetPlayerState(PlayerController, _cameraController));
        }
    }
}