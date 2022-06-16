using UnityEngine;

namespace Utils
{
    public struct PlanetCell
    {
        public readonly Vector2 rangeX;
        public readonly Vector2 rangeY;
        public readonly Vector2 rangeZ;

        public PlanetCell(Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ)
        {
            IsOccupied = false;
            this.rangeX = rangeX;
            this.rangeY = rangeY;
            this.rangeZ = rangeZ;
        }

        public bool IsOccupied { get; private set; }

        public void Occupied()
        {
            IsOccupied = true;
        }
    }
}