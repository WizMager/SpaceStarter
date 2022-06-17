using System;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/Data", fileName = "Data")]
    public class AllData : ScriptableObject
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlanetData _planetData;
        [SerializeField] private CameraData _cameraData;
        [SerializeField] private InputData _inputData;
        [SerializeField] private PrefabLinkData _prefabLinkData;
        [SerializeField] private PortalData _portalData;
        [SerializeField] private ObjectsOnPlanetData _objectsOnPlanetData;
        [SerializeField] private RestartData _restartData;
        [SerializeField] private MissileData _missileData;
        [SerializeField] private MaterialsData _materialsData;

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

        public RestartData RestartData
        {
            get
            {
                if (_restartData == null)
                {
                    throw new NullReferenceException("You don't create or set RestartData scriptable object");
                }

                return _restartData;
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

        public PrefabLinkData Prefab
        {
            get
            {
                if (_prefabLinkData == null)
                {
                    throw new NullReferenceException("You don't create or set PrefabLinkData scriptable object");
                }

                return _prefabLinkData;
            }
        }
        
        public PortalData Portal
        {
            get
            {
                if (_portalData == null)
                {
                    throw new NullReferenceException("You don't create or set PortalData scriptable object");
                }

                return _portalData;
            }
        }        
        
        public ObjectsOnPlanetData ObjectsOnPlanetData
        {
            get
            {
                if (_objectsOnPlanetData == null)
                {
                    throw new NullReferenceException("You don't create or set ObjectsOnPlanetData scriptable object");
                }

                return _objectsOnPlanetData;
            }
        }
        public MissileData MissileData
        {
            get
            {
                if (_missileData == null)
                {
                    throw new NullReferenceException("You don't create or set MissileData scriptable object");
                }

                return _missileData;
            }
        }
        
        public MaterialsData Materials
        {
            get
            {
                if (_materialsData == null)
                {
                    throw new NullReferenceException("You don't create or set MaterialsData scriptable object");
                }

                return _materialsData;
            }
        }
    }
}