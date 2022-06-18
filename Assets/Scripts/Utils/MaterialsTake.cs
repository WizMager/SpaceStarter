using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

namespace Utils
{
    public class MaterialsTake
    {
        private readonly MaterialsData _materialsData;

        public MaterialsTake(MaterialsData materialsData)
        {
            _materialsData = materialsData;
        }

        public Dictionary<int, Dictionary<int, List<Material>>> TakeRandomMaterials(int randomColorNumber)
        {
            var preparedMaterials = new Dictionary<int, Dictionary<int, List<Material>>>(7);
            for (int i = 0; i < _materialsData.floor.Count; i++)
            {
                var currentTypeLength = _materialsData.floor[i].materials.Count;
                var chosenMaterials = new List<Material>
                {
                    [0] = _materialsData.floor[i].materials[randomColorNumber],
                    [1] = _materialsData.floor[i].materials[randomColorNumber + currentTypeLength / 4],
                    [2] = _materialsData.floor[i].materials[randomColorNumber + currentTypeLength / 4 * 2],
                    [3] = _materialsData.floor[i].materials[randomColorNumber + currentTypeLength / 4 * 3]
                };
                preparedMaterials[0].Add(i, chosenMaterials);
            }

            for (int i = 0; i < _materialsData.planet.Count; i++)
            {
                var currentTypeLength = _materialsData.planet[i].materials.Count;
                var chosenMaterials = new List<Material>(3)
                {
                    [0] = _materialsData.planet[i].materials[randomColorNumber],
                    [1] = _materialsData.planet[i].materials[randomColorNumber + currentTypeLength / 3],
                    [2] = _materialsData.planet[i].materials[randomColorNumber + currentTypeLength / 3 * 2]
                };
                preparedMaterials[1].Add(i, chosenMaterials);
            }
            
            for (int i = 0; i < _materialsData.tree.Count; i++)
            {
                var currentTypeLength = _materialsData.tree[i].materials.Count;
                var chosenMaterials = new List<Material>(2)
                {
                    [0] = _materialsData.tree[i].materials[randomColorNumber],
                    [1] = _materialsData.tree[i].materials[randomColorNumber + currentTypeLength / 2]
                };
                preparedMaterials[2].Add(i, chosenMaterials);
            }

            for (int i = 0; i < _materialsData.chelik.Count; i++)
            {
                var chosenMaterials = new List<Material>(1)
                {
                    [0] = _materialsData.chelik[i].materials[randomColorNumber],
                };
                preparedMaterials[3].Add(i, chosenMaterials);
            }
            
            for (int i = 0; i < _materialsData.atmosphere.Count; i++)
            {
                var chosenMaterials = new List<Material>(1)
                {
                    [0] = _materialsData.atmosphere[i].materials[randomColorNumber],
                };
                preparedMaterials[4].Add(i, chosenMaterials);
            }
            
            return preparedMaterials;
        }
    }
}