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
            _houseBuilder.ResetHouse();
            _houseBuilder.CreateSimpleFloor();
            _houseBuilder.CreateSimpleFloor();
            _houseBuilder.CreateRoof();
            return _houseBuilder.GetHouse();
        }
    }
}