using System.Collections.Generic;
using Builders;
using ScriptableData;
using UnityEngine;
using View;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Controllers

{
    public class BuildingsController
    {
        private readonly AllData _data;
        private readonly Transform _planet;
        private readonly float _planetRadius;
        private readonly Transform _positionGenerator;
        private readonly float _minimumAngleBetweenBuildings;
        private readonly float _maximumAngleBetweenBuildings;
        private readonly float _maximumBuildingAngleUp;
        private readonly float _maximumBuildingAngleDown;
        private readonly float _maximumAngleRotateBuildingAroundItself;
        
        private readonly GameObject _buildingPrefab;
        private int _buildingsCounter;
        private readonly GameObject _rootBuildingAroundPlanet;
        private FirstTypeHouseBuilder _firstTypeHouseBuilder;

        public BuildingsController(AllData data, Transform planet, Transform positionGenerator)
        {
            _data = data;
            _planet = planet;
            _planetRadius = planet.GetComponent<SphereCollider>().radius;
            _positionGenerator = positionGenerator;
            _minimumAngleBetweenBuildings = data.ObjectsOnPlanetData.minimalAngleBetweenBuildings;
            _maximumAngleBetweenBuildings = data.ObjectsOnPlanetData.maximumAngleBetweenBuildings;
            _maximumBuildingAngleUp = data.ObjectsOnPlanetData.maximumBuildingAngleUp;
            _maximumBuildingAngleDown = data.ObjectsOnPlanetData.maximumBuildingAngleDown;
            _maximumAngleRotateBuildingAroundItself = data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItself;
            
            _buildingPrefab = Resources.Load<GameObject>("TestBuilding");
            var rootEnvironment = new GameObject("PlanetEnvironment");
            _rootBuildingAroundPlanet = new GameObject("BuildingAroundPlanet");
            _rootBuildingAroundPlanet.transform.SetParent(rootEnvironment.transform);
            _firstTypeHouseBuilder = new FirstTypeHouseBuilder();
        }

        #region Generate_Buildings_Around_Planet

        public void GeneratePositions()
        {
            var planetPosition = _planet.position;
            var ray = new Ray(planetPosition, _planet.forward);
            _positionGenerator.position = ray.GetPoint(_planetRadius);
            for (float i = 0; i < 360f; )
            {
                var iterationAngle = Random.Range(_minimumAngleBetweenBuildings, _maximumAngleBetweenBuildings);
                if (360f - i < _minimumAngleBetweenBuildings)
                {
                    Debug.Log(_buildingsCounter);
                    _firstTypeHouseBuilder.CreateSimpleFloor();
                    _firstTypeHouseBuilder.CreateSimpleFloor();
                    _firstTypeHouseBuilder.CreateGlassFloor();
                    var go = _firstTypeHouseBuilder.GetHouse();
                    go.transform.RotateAround(go.transform.position, go.transform.forward, Random.Range(0f, _maximumAngleRotateBuildingAroundItself));
                    return;
                }
                i += iterationAngle;
                _positionGenerator.RotateAround(planetPosition, _planet.up, iterationAngle);
                var upOrDownAngle = Random.Range(-_maximumBuildingAngleDown, _maximumBuildingAngleUp);
                _positionGenerator.RotateAround(planetPosition, _planet.forward, upOrDownAngle);
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var building = Object.Instantiate(_buildingPrefab, _positionGenerator.position, _positionGenerator.rotation);
                building.transform.RotateAround(building.transform.position, building.transform.forward, randomAngleRotationBuilding);
                building.transform.SetParent(_rootBuildingAroundPlanet.transform);
                _buildingsCounter++;
                _positionGenerator.RotateAround(planetPosition, _planet.forward, -upOrDownAngle);
            }
        }

        #endregion
        
        private void CreateBuildingsByPrefab(Transform planet, GameObject buildingPrefab, int buildingsCount)
        {
            
            var buildingHeight = buildingPrefab.GetComponent<BoxCollider>().size.y;
            planet.GetComponent<SphereCollider>();
            for (int i = 1; i <= buildingsCount; i++)
            {
                var x = Random.Range(0f, 360f);
                var y = Random.Range(0f, 360f);
                var z = Random.Range(0f, 360f);
                Quaternion rotation = Quaternion.Euler(x, y, z);
                Vector3 position = planet.position + rotation * Vector3.up * (_planetRadius + buildingHeight / 2);
                Object.Instantiate(buildingPrefab, position, rotation);
            }
        }
        
        public void CreateBuildings(Transform planet)
        {
            var objectTypes = _data.ObjectsOnPlanetData.objectsTypeOnPlanet;
            foreach (var objectType in objectTypes)
            {
                CreateBuildingsByPrefab(planet, objectType.prefab, objectType.count);
            }
        }
    }
}
