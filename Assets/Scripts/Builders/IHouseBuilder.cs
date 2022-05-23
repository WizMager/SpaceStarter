using UnityEngine;

namespace Builders
{
    public interface IHouseBuilder
    {
        //bool IsDestructible();
        void ResetHouse();
        void CreateSimpleFloor();
        void CreateGlassFloor();
        void CreateRoof();
        void CreateGlassRoof();
        GameObject GetHouse();
    }
}