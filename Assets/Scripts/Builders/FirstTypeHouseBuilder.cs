using UnityEngine;

namespace Builders
{
    public class FirstTypeHouseBuilder : IHouseBuilder
    {
        private GameObject _house;
        private readonly GameObject _simpleFloor;
        private readonly GameObject _glassFloor;
        private readonly GameObject _roof;

        private int _houseNumber;
        private const string HouseName = "House";
        private float _localPositionZ;

        public FirstTypeHouseBuilder()
        {
            _simpleFloor = Resources.Load<GameObject>("SimpleFloor");
            _glassFloor = Resources.Load<GameObject>("GlassFloor");
            _roof = Resources.Load<GameObject>("Roof");
            _house = new GameObject(HouseName + _houseNumber);
        }

        private void ResetHouse()
        {
            _houseNumber++;
            _house = new GameObject(HouseName + _houseNumber);
        }
        
        public void CreateSimpleFloor()
        {
            var simpleFloor = Object.Instantiate(_simpleFloor, _house.transform, false);
            var sizeY = simpleFloor.GetComponent<MeshRenderer>().bounds.size.y;
            var position = new Vector3(0, 0, _localPositionZ + sizeY / 2);
            simpleFloor.transform.localPosition = position; 
            _localPositionZ += sizeY;
        }

        public void CreateGlassFloor()
        {
            var glassFloor = Object.Instantiate(_glassFloor, _house.transform, false);
            var sizeY = glassFloor.GetComponent<MeshRenderer>().bounds.size.y;
            var position = new Vector3(0, 0, _localPositionZ + sizeY / 2);
            glassFloor.transform.localPosition = position; 
            _localPositionZ += sizeY;
        }

        public GameObject RoofAndGetHouse()
        {
            var roof = Object.Instantiate(_roof, _house.transform, false);
            var sizeY = roof.GetComponent<MeshRenderer>().bounds.size.y;
            var position = new Vector3(0, 0, _localPositionZ + sizeY / 2);
            roof.transform.localPosition = position; 
            _localPositionZ += sizeY;
            
            var completeHouse = _house;
            ResetHouse();
            return completeHouse;
        }
    }
}