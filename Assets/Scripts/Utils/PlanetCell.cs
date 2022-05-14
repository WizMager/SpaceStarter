using UnityEngine;

namespace Utils
{
    public struct PlanetCell
    {
        public bool isOccupied;
        public Vector2 rangeX;
        public Vector2 rangeY;
        public Vector2 rangeZ;

        public void Occupied()
        {
            isOccupied = true;
        }
    }
}