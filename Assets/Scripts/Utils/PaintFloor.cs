using System;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using View;
using Random = UnityEngine.Random;

namespace Utils
{
    public class PaintFloor
    {
        private readonly List<Material[]> _floorsMaterials;
        private int _currentColor;

        public PaintFloor(MaterialsData materialsData, int houseTypeNumber)
        {
            _floorsMaterials = new List<Material[]>(GetFloorsMaterials(materialsData, houseTypeNumber));
            _currentColor = Random.Range(0, _floorsMaterials[1].Length / 4 - 1);
        }
        
        private List<Material[]> GetFloorsMaterials(MaterialsData materialsData, int houseTypeNumber)
        {
            var floorsMaterials = new List<Material[]>
            {
                materialsData.glassHouse,
                GetSelectedTypeMaterials(materialsData, houseTypeNumber)
            };

            return floorsMaterials;
        }

        private Material[] GetSelectedTypeMaterials(MaterialsData materialsData, int houseTypeNumber)
        {
            Material[] materialSelectedType;
            switch (houseTypeNumber)
            {
                case 1:
                    materialSelectedType = materialsData.house1Type;
                    break;
                case 2:
                    materialSelectedType = materialsData.house2Type;
                    break;
                case 3:
                    materialSelectedType = materialsData.house3Type;
                    break;
                case 4:
                    materialSelectedType = materialsData.house4Type;
                    break;
                case 5:
                    materialSelectedType = materialsData.house5Type;
                    break;
                case 6:
                    materialSelectedType = materialsData.house6Type;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        "Out of range number floor materials types");
            }

            return materialSelectedType;
        }

        public void Paint(GameObject floor, bool isGlassFloor)
        {
            var meshRenderers = floor.GetComponentsInChildren<MeshRenderer>();
            if (isGlassFloor)
            {
                foreach (var meshRenderer in meshRenderers)
                {
                    var floorMaterials = _floorsMaterials[0];
                    if (meshRenderer.gameObject.CompareTag("BlockFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor];
                    }

                    if (meshRenderer.gameObject.CompareTag("WindowFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor + 1];
                    }

                    if (meshRenderer.gameObject.CompareTag("WindowFrameFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor + 2];
                    }
                    
                    if (meshRenderer.gameObject.CompareTag("OtherPartFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor + 3];
                    }
                } 
            }
            else
            {
                foreach (var meshRenderer in meshRenderers)
                {
                    var floorMaterials = _floorsMaterials[1];
                    if (meshRenderer.gameObject.CompareTag("BlockFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor];
                    }

                    if (meshRenderer.gameObject.CompareTag("WindowFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor + 1];
                    }
                    
                    if (meshRenderer.gameObject.CompareTag("WindowFrameFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor + 2];
                    }
                    
                    if (meshRenderer.gameObject.CompareTag("OtherPartFloor"))
                    {
                        meshRenderer.material = floorMaterials[_currentColor + 3];
                    }
                } 
            }
        }

        public void ChangeColor()
        {
            var previousColor = _currentColor;
            do
            { 
                _currentColor = Random.Range(0, _floorsMaterials[1].Length / 4 - 1);
            } 
            while (_currentColor != previousColor);
        }
    }
}