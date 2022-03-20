﻿using System;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/Data", fileName = "Data")]
    public class ScriptableData : ScriptableObject
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private LastPlanetData _lastPlanetData;
        [SerializeField] private PlanetData _planetData;
        [SerializeField] private CameraData _cameraData;
        [SerializeField] private InputData _inputData;
        [SerializeField] private BonusData _bonusData;

        public PlayerData Player
        {
            get
            {
                if (_playerData == null)
                {
                    throw new NullReferenceException("You don't create or set PlayerData scriptable object");
                }

                return _playerData;
            }
        }

        public LastPlanetData LastPlanet
        {
            get
            {
                if (_lastPlanetData == null)
                {
                    throw new NullReferenceException("You don't create or set LastPlanetData scriptable object");
                }

                return _lastPlanetData;
            }
        }
        
        public PlanetData Planet
        {
            get
            {
                if (_planetData == null)
                {
                    throw new NullReferenceException("You don't create or set PlanetData scriptable object");
                }

                return _planetData;
            }
        }
        
        public CameraData Camera
        {
            get
            {
                if (_cameraData == null)
                {
                    throw new NullReferenceException("You don't create or set CameraData scriptable object");
                }

                return _cameraData;
            }
        }

        public InputData Input
        {
            get
            {
                if (_inputData == null)
                {
                    throw new NullReferenceException("You don't create or set InputData scriptable object");
                }

                return _inputData;
            }
        }
        
        public BonusData Bonus
        {
            get
            {
                if (_bonusData == null)
                {
                    throw new NullReferenceException("You don't create or set BonusData scriptable object");
                }

                return _bonusData;
            }
        }
    }
}