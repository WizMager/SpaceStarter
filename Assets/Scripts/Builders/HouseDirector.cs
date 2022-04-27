using UnityEngine;

namespace Builders
{
    public class HouseDirector
    {
        private IHouseBuilder _houseBuilder;
        
        public IHouseBuilder Builder
        {
            set => _houseBuilder = value;
        }

        public GameObject Build3Floor()
        {
            _houseBuilder.CreateSimpleFloor();
            _houseBuilder.CreateSimpleFloor();
            return _houseBuilder.RoofAndGetHouse();
        }
    }
}