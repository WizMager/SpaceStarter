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

        public GameObject BuildSimpleHouse(int floors)
        {
            _houseBuilder.ResetHouse();
            for (int i = 0; i < floors; i++)
            {
                _houseBuilder.CreateSimpleFloor();  
            }
            _houseBuilder.CreateRoof();
            return _houseBuilder.GetHouse();
        }
        
        public GameObject BuildGlassHouse(int floors)
        {
            _houseBuilder.ResetHouse();
            
            return _houseBuilder.GetHouse();
        }
    }
}