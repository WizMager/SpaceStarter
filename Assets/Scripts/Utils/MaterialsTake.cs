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
            preparedMaterials.Add(0, new Dictionary<int, List<Material>>(7));
            for (int i = 0; i < _materialsData.floor.Count; i++)
            {
                var currentTypeLength = _materialsData.floor[i].materials.Count;
                var chosenMaterials = new List<Material>();
                chosenMaterials.Add(_materialsData.floor[i].materials[randomColorNumber]);
                chosenMaterials.Add(_materialsData.floor[i].materials[randomColorNumber + currentTypeLength / 4]);
                chosenMaterials.Add(_materialsData.floor[i].materials[randomColorNumber + currentTypeLength / 4 * 2]);
                chosenMaterials.Add(_materialsData.floor[i].materials[randomColorNumber + currentTypeLength / 4 * 3]);
                preparedMaterials[0].Add(i, chosenMaterials);
            }

            preparedMaterials.Add(1, new Dictionary<int, List<Material>>(1));
            for (int i = 0; i < _materialsData.planet.Count; i++)
            {
                var currentTypeLength = _materialsData.planet[i].materials.Count;
                var chosenMaterials = new List<Material>();
                chosenMaterials.Add(_materialsData.planet[i].materials[randomColorNumber]);
                chosenMaterials.Add(_materialsData.planet[i].materials[randomColorNumber + currentTypeLength / 3]);
                chosenMaterials.Add(_materialsData.planet[i].materials[randomColorNumber + currentTypeLength / 3 * 2]);
                preparedMaterials[1].Add(i, chosenMaterials);
            }
            
            preparedMaterials.Add(2, new Dictionary<int, List<Material>>(4));
            for (int i = 0; i < _materialsData.tree.Count; i++)
            {
                var currentTypeLength = _materialsData.tree[i].materials.Count;
                var chosenMaterials = new List<Material>();
                chosenMaterials.Add(_materialsData.tree[i].materials[randomColorNumber]);
                chosenMaterials.Add(_materialsData.tree[i].materials[randomColorNumber + currentTypeLength / 2]);
                preparedMaterials[2].Add(i, chosenMaterials);
            }

            preparedMaterials.Add(3, new Dictionary<int, List<Material>>(1));
            for (int i = 0; i < _materialsData.chelik.Count; i++)
            {
                var chosenMaterials = new List<Material>();
                chosenMaterials.Add(_materialsData.chelik[i].materials[randomColorNumber]);
                preparedMaterials[3].Add(i, chosenMaterials);
            }
            
            preparedMaterials.Add(4, new Dictionary<int, List<Material>>(1));
            for (int i = 0; i < _materialsData.atmosphere.Count; i++)
            {
                var chosenMaterials = new List<Material>();
                chosenMaterials.Add(_materialsData.atmosphere[i].materials[randomColorNumber]);
                preparedMaterials[4].Add(i, chosenMaterials);
            }
            
            return preparedMaterials;
        }
    }
}