using System.Collections.Generic;
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
        private readonly float _minimalAngle;
        private readonly float _maximumAngle;
        
        private readonly GameObject _buildingPrefab;

        public BuildingsController(AllData data, Transform planet, Transform positionGenerator)
        {
            _data = data;
            _planet = planet;
            _planetRadius = planet.GetComponent<SphereCollider>().radius;
            _positionGenerator = positionGenerator;
            _minimalAngle = data.ObjectsOnPlanetData.minimalAngleBetweenBuildings;
            _maximumAngle = data.ObjectsOnPlanetData.maximumAngleBetweenBuildings;
            
            _buildingPrefab = Resources.Load<GameObject>("TestBuilding");
        }

        #region Generate_Buildings_Around_Planet

        public void GeneratePositions()
        {
            var ray = new Ray(_planet.position, _planet.forward);
            _positionGenerator.position = ray.GetPoint(_planetRadius);
            var angleAroundPlanet = 360f - (_maximumAngle - _minimalAngle);
            for (float i = 0; i < angleAroundPlanet; )
            {
                var iterationAngle = Random.Range(_minimalAngle, _maximumAngle);
                i += iterationAngle;
                _positionGenerator.RotateAround(_planet.position, _planet.up, iterationAngle);
                Object.Instantiate(_buildingPrefab, -_positionGenerator.position, _positionGenerator.rotation);
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
