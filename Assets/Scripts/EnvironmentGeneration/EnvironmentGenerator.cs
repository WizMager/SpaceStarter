using System.Collections.Generic;
using ScriptableData;
using UnityEngine;


namespace EnvironmentGeneration
{
    public class EnvironmentGenerator
    {
        private readonly BuildingAroundPlanetGenerator _buildingAroundPlanetGenerator;

        public EnvironmentGenerator(AllData data, Transform planet)
        {
            var rootEnvironment = new GameObject("PlanetEnvironment");
            var planetRadius = planet.GetComponent<SphereCollider>().radius;
            _buildingAroundPlanetGenerator = new BuildingAroundPlanetGenerator(data, planet, planetRadius, rootEnvironment);
        }

        public List<Transform> GenerateBuildingsAroundPlanet()
        {
            return _buildingAroundPlanetGenerator.GenerateBuildingsAroundPlanet();
        }
        
    }
}