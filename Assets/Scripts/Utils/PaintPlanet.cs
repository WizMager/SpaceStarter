using System;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public class PaintPlanet
    {
        private Material[] _grassPlanet;
        private Material[] _groundPlanet;
        private Material[] _waterPlanet;
        
        public PaintPlanet(MaterialsData materialsData)
        {
            GetPlanetMaterials(materialsData);
        }
        
        private void GetPlanetMaterials(MaterialsData materialsData)
        {
            _grassPlanet = new Material[materialsData.grassPlanet.Length];
            for (int i = 0; i < _grassPlanet.Length; i++)
            {
                _grassPlanet[i] = materialsData.grassPlanet[i];
            }

            _groundPlanet = new Material[materialsData.groundPlanet.Length];
            for (int i = 0; i < _groundPlanet.Length; i++)
            {
                _groundPlanet[i] = materialsData.groundPlanet[i];
            }
            
            _waterPlanet = new Material[materialsData.waterPlanet.Length];
            for (int i = 0; i < _waterPlanet.Length; i++)
            {
                _waterPlanet[i] = materialsData.waterPlanet[i];
            }
        }
        
        public void Paint(List<Transform> planetPieces)
        {
            var colorTypeNumber = Random.Range(0, _grassPlanet.Length);
            foreach (var transform in planetPieces)
            {
                var meshRenderer = transform.GetComponent<MeshRenderer>();
                var pieceTag = transform.tag;
            
                if (pieceTag == "PlanetGrass")
                {
                    if (colorTypeNumber > _grassPlanet.Length - 1)
                    {
                        throw new ArgumentOutOfRangeException("Random number more then length of _grassPlanet.");
                    }
                    meshRenderer.material = _grassPlanet[colorTypeNumber];
                }
            
                if (pieceTag == "PlanetGround")
                {
                    if (colorTypeNumber > _groundPlanet.Length - 1)
                    {
                        throw new ArgumentOutOfRangeException("Random number more then length of _groundPlanet.");
                    }
                    meshRenderer.material = _groundPlanet[colorTypeNumber];
                }
            
                if (pieceTag == "PlanetWater")
                {
                    if (colorTypeNumber > _waterPlanet.Length - 1)
                    {
                        throw new ArgumentOutOfRangeException("Random number more then length of _waterPlanet.");
                    }
                    meshRenderer.material = _waterPlanet[colorTypeNumber];
                }
            }
        }
    }
}