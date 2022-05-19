using UnityEngine;

namespace Utils
{
    public struct PlanetCell
    {
        private bool _isOccupied;
        public readonly Vector2 rangeX;
        public readonly Vector2 rangeY;
        public readonly Vector2 rangeZ;

        public PlanetCell(Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ)
        {
            _isOccupied = false;
            this.rangeX = rangeX;
            this.rangeY = rangeY;
            this.rangeZ = rangeZ;
        }

        public bool IsOccupied => _isOccupied;
        
        public void Occupied()
        {
            _isOccupied = true;
        }
    }
}