using ScriptableData;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Controllers

{
    public class BuildingsController
    {
        private readonly AllData _data;
        private readonly float _planetRadius;

        public BuildingsController(AllData data, Transform planet)
        {
            _data = data;
            _planetRadius = planet.GetComponent<SphereCollider>().radius;
        }

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
