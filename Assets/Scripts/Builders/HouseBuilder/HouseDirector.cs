﻿using UnityEngine;

namespace Builders.HouseBuilder
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
            var glassFloorNumber = Random.Range(0, floors + 1);
            if (glassFloorNumber == floors + 1)
            {
                for (int i = 0; i < floors; i++)
                {
                    _houseBuilder.CreateSimpleFloor(); 
                }
                _houseBuilder.CreateGlassRoof();
            }
            else
            {
                for (int i = 0; i < floors; i++)
                {
                    if (i == glassFloorNumber)
                    {
                        _houseBuilder.CreateGlassFloor();
                    }
                    else
                    {
                        _houseBuilder.CreateSimpleFloor();
                    }
                }
                _houseBuilder.CreateRoof();
            }
            return _houseBuilder.GetHouse();
        }
    }
}