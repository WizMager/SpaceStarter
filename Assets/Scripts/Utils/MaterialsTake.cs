using System;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public class MaterialsTake
    {
        private readonly List<Material[]> _treesMaterials;

        public List<Material[]> TreesMaterial => _treesMaterials;

        public MaterialsTake(AllData data)
        {
            _treesMaterials = GetTreesMaterials(data.Prefab.trees.Length, data.Materials);
        }
        
        private List<Material[]> GetTreesMaterials(int treePrefabs, MaterialsData materialsData)
        {
            var type1 = materialsData.tree1Type;
            var type1Length = materialsData.tree1Type.Length;
            var treesMaterials = new List<Material[]>
            {
                materialsData.tree1Type,
                materialsData.tree2Type,
                materialsData.tree3Type,
                materialsData.tree4Type
            };
            if (treesMaterials.Count != treePrefabs)
            {
                throw new ArgumentOutOfRangeException(
                    "Number of tree prefabs is does not mach with number materials types");
            }

            return treesMaterials;
        }

        private Material[] TakeRandomMaterialForType(Material[] materials)
        {
            var length = materials.Length;
            var randomColorNumber = Random.Range(0, length / 2);
            var takeMaterial = new Material[2];
            takeMaterial[0] = materials[randomColorNumber];
            takeMaterial[1] = materials[randomColorNumber + length / 2];

        }
    }
}