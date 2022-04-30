using UnityEngine;
using View;

namespace Builders
{
    public class ThirdTypeHouseBuilder : IHouseBuilder
    {
        private GameObject _house;
        private readonly GameObject _simpleFloor;
        private readonly GameObject _glassFloor;
        private readonly GameObject _roof;
        private readonly GameObject _glassRoof;

        private int _houseNumber;
        private int _floorNumber;
        private const string FloorName = "Floor";
        private const string HouseName = "HouseThirdType";
        private float _localPositionZ;

        public ThirdTypeHouseBuilder()
        {
            _simpleFloor = Resources.Load<GameObject>("Buildings/ThirdTypeBuilding/SimpleFloorType3");
            _glassFloor = Resources.Load<GameObject>("Buildings/GlassBuilding/GlassFloor");
            _roof = Resources.Load<GameObject>("Buildings/ThirdTypeBuilding/RoofType3");
            _glassRoof = Resources.Load<GameObject>("Buildings/GlassBuilding/GlassRoof");
        }

        public void ResetHouse()
        {
            _localPositionZ = 0f;
            _floorNumber = 0;
            _houseNumber++;
            _house = new GameObject(HouseName + _houseNumber);
        }
        
        public void CreateSimpleFloor()
        {
            var simpleFloor = Object.Instantiate(_simpleFloor, _house.transform, false);
            simpleFloor.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = simpleFloor.GetComponent<BoxCollider>().bounds.size.y;
            var position = new Vector3(0, 0, _localPositionZ + sizeY / 2);
            simpleFloor.transform.localPosition = position; 
            _localPositionZ += sizeY;
        }

        public void CreateGlassFloor()
        {
            var glassFloor = Object.Instantiate(_glassFloor, _house.transform, false);
            glassFloor.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = glassFloor.GetComponent<MeshRenderer>().bounds.size.y;
            var position = new Vector3(0, 0, _localPositionZ + sizeY / 2);
            glassFloor.transform.localPosition = position; 
            _localPositionZ += sizeY;
        }

        public void CreateRoof()
        {
            var roof = Object.Instantiate(_roof, _house.transform, false);
            roof.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = roof.GetComponent<MeshRenderer>().bounds.size.y;
            var position = new Vector3(0, 0, _localPositionZ + sizeY / 2);
            roof.transform.localPosition = position; 
            _localPositionZ += sizeY;
        }

        public void CreateGlassRoof()
        {
            var roof = Object.Instantiate(_glassRoof, _house.transform, false);
            roof.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = roof.GetComponent<MeshRenderer>().bounds.size.y;
            var position = new Vector3(0, 0, _localPositionZ + sizeY / 2);
            roof.transform.localPosition = position; 
            _localPositionZ += sizeY;
        }

        public GameObject GetHouse()
        {
            var completeHouse = _house;
            completeHouse.AddComponent<BuildingView>();
            return completeHouse;
        }
    }
}