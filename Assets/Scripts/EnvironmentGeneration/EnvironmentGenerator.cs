using System.Collections.Generic;
using System.Linq;
using ScriptableData;
using UnityEngine;


namespace EnvironmentGeneration
{
    public class EnvironmentGenerator
    {
        private readonly List<Transform> _allEnvironment;

        private readonly BuildingAroundPlanetGenerator _buildingAroundPlanetGenerator;
        private readonly BuildingOnPlanetGenerator _buildingOnPlanetGenerator;

        public EnvironmentGenerator(AllData data, Transform planet)
        {
            _allEnvironment = new List<Transform>();
            var rootEnvironment = new GameObject("PlanetEnvironment");
            var planetRadius = planet.GetComponent<SphereCollider>().radius;
            _buildingAroundPlanetGenerator = new BuildingAroundPlanetGenerator(data, planet, planetRadius, rootEnvironment);
            _buildingOnPlanetGenerator = new BuildingOnPlanetGenerator(data, planet, planetRadius, rootEnvironment);
        }

        public List<Transform> GenerateEnvironment()
        {
            var buildingsAroundPlanet = _buildingAroundPlanetGenerator.GenerateBuildingsAroundPlanet();
            _allEnvironment.Union(buildingsAroundPlanet);
            var numberBuildingsAroundPlanet = _buildingAroundPlanetGenerator.BuildingsSpawned;
            _buildingOnPlanetGenerator.CreateBuildingAndPosition(numberBuildingsAroundPlanet);
            return _allEnvironment;
        }

    }
}