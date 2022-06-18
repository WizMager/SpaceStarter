using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class PaintFloor
    {
        private readonly List<Material> _glassFloor;
        private readonly List<Material> _simpleFloor;

        public PaintFloor(List<Material> glassFloor, List<Material> simpleFloor)
        {
            _glassFloor = glassFloor;
            _simpleFloor = simpleFloor;
        }

        public void Paint(GameObject floor, bool isGlassFloor)
        {
            var meshRenderers = floor.GetComponentsInChildren<MeshRenderer>();
            if (isGlassFloor)
            {
                foreach (var meshRenderer in meshRenderers)
                {
                    if (meshRenderer.gameObject.CompareTag("BlockFloor"))
                    {
                        meshRenderer.material = _glassFloor[0];
                    }

                    if (meshRenderer.gameObject.CompareTag("WindowFloor"))
                    {
                        meshRenderer.material = _glassFloor[1];
                    }

                    if (meshRenderer.gameObject.CompareTag("WindowFrameFloor"))
                    {
                        meshRenderer.material = _glassFloor[2];
                    }
                    
                    if (meshRenderer.gameObject.CompareTag("OtherPartFloor"))
                    {
                        meshRenderer.material = _glassFloor[3];
                    }
                } 
            }
            else
            {
                foreach (var meshRenderer in meshRenderers)
                {
                    if (meshRenderer.gameObject.CompareTag("BlockFloor"))
                    {
                        meshRenderer.material = _simpleFloor[0];
                    }

                    if (meshRenderer.gameObject.CompareTag("WindowFloor"))
                    {
                        meshRenderer.material = _simpleFloor[1];
                    }
                    
                    if (meshRenderer.gameObject.CompareTag("WindowFrameFloor"))
                    {
                        meshRenderer.material = _simpleFloor[2];
                    }
                    
                    if (meshRenderer.gameObject.CompareTag("OtherPartFloor"))
                    {
                        meshRenderer.material = _simpleFloor[3];
                    }
                } 
            }
        }
    }
}