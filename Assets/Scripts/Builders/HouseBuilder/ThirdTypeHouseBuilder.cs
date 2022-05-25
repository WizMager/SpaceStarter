using UnityEngine;
using View;

namespace Builders.HouseBuilder
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
        private float _localPositionY;

        public ThirdTypeHouseBuilder()
        {
            _simpleFloor = Resources.Load<GameObject>("Buildings/House3Type/House3Floor");
            _glassFloor = Resources.Load<GameObject>("Buildings/GlassBuilding/GlassFloor");
            _roof = Resources.Load<GameObject>("Buildings/House3Type/House3Roof");
            _glassRoof = Resources.Load<GameObject>("Buildings/GlassBuilding/GlassRoof");
        }

        public void ResetHouse()
        {
            _localPositionY = 0f;
            _floorNumber = 0;
            _houseNumber++;
            _house = new GameObject(HouseName + _houseNumber);
        }
        
        public void CreateSimpleFloor()
        {
            var simpleFloor = Object.Instantiate(_simpleFloor, _house.transform, false);
            simpleFloor.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = simpleFloor.GetComponentInChildren<BoxCollider>().bounds.size.y;
            var position = new Vector3(0, _localPositionY + sizeY / 2, 0);
            simpleFloor.transform.localPosition = position;
            var rotation = Quaternion.Euler(0f, 0f, 0f);
            simpleFloor.transform.localRotation = rotation;
            _localPositionY += sizeY;
        }

        public void CreateGlassFloor()
        {
            var glassFloor = Object.Instantiate(_glassFloor, _house.transform, false);
            glassFloor.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = glassFloor.GetComponent<BoxCollider>().bounds.size.y;
            var position = new Vector3(0, _localPositionY + sizeY / 2, 0);
            glassFloor.transform.localPosition = position;
            var rotation = Quaternion.Euler(0f, 0f, 0f);
            glassFloor.transform.localRotation = rotation;
            _localPositionY += sizeY;
        }

        public void CreateRoof()
        {
            var roof = Object.Instantiate(_roof, _house.transform, false);
            roof.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = roof.GetComponentInChildren<BoxCollider>().bounds.size.y;
            var position = new Vector3(0, _localPositionY + sizeY / 2, 0);
            roof.transform.localPosition = position;
            var rotation = Quaternion.Euler(180f, 0f, 0f);
            roof.transform.localRotation = rotation;
            _localPositionY += sizeY;
        }

        public void CreateGlassRoof()
        {
            var roof = Object.Instantiate(_glassRoof, _house.transform, false);
            roof.name = FloorName + _floorNumber;
            _floorNumber++;
            var sizeY = roof.GetComponent<BoxCollider>().bounds.size.y;
            var position = new Vector3(0, _localPositionY + sizeY / 2, 0);
            roof.transform.localPosition = position;
            var rotation = Quaternion.Euler(0f, 0f, 0f);
            roof.transform.localRotation = rotation;
            _localPositionY += sizeY;
        }

        public GameObject GetHouse()
        {
            var completeHouse = _house;
            completeHouse.AddComponent<BuildingView>();
            return completeHouse;
        }
    }
}