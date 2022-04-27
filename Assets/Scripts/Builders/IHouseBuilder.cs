using UnityEngine;

namespace Builders
{
    public interface IHouseBuilder
    {
        void CreateSimpleFloor();
        void CreateGlassFloor();
        GameObject RoofAndGetHouse();
    }
}