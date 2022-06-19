using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class PaintPlanet
    {
        private readonly List<Material> _materials;

        public PaintPlanet(List<Material> materials)
        {
            _materials = materials;
        }

        public void Paint(List<Transform> planetPieces)
        {
            foreach (var transform in planetPieces)
            {
                var meshRenderer = transform.GetComponent<MeshRenderer>();
                var pieceTag = transform.tag;
            
                if (pieceTag == "PlanetGrass")
                {
                    meshRenderer.material = _materials[0];
                }
            
                if (pieceTag == "PlanetGround")
                {
                    meshRenderer.material = _materials[1];
                }
            
                if (pieceTag == "PlanetWater")
                {
                    meshRenderer.material = _materials[2];
                }
            }
        }
    }
}