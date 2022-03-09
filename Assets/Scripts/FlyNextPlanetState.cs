﻿using UnityEngine;

namespace DefaultNamespace
{
    public class FlyNextPlanetState : State
    {
        public FlyNextPlanetState(Vector3 direction)
        {
            _stateContext.SetDirectionToEdge(direction);
        }
        
        public override void Move(float deltaTime)
        {
            if (!_stateContext.FlyToEdgeGravity()) return;
            _stateContext.ChangeCurrentPlanet();
            _stateContext.TransitionTo(new FlyAroundPlanetState());
        }
    }
}