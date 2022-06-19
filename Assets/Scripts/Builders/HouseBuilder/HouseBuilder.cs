using UnityEngine;
using View;
using Utils;
using System;
using System.Collections.Generic;

namespace Builders.HouseBuilder
{
    public class HouseBuilder : IHouseBuilder
    {
        private GameObject _house;
        private readonly GameObject _simpleFloor;
        private readonly GameObject _glassFloor;
        private readonly GameObject _roof;
        private readonly GameObject _glassRoof;

        private readonly int _houseTypeNumber;
        private int _houseNumber;
        private int _floorNumber;
        private const string FloorName = "Floor";
        private const string HouseName = "HouseType";
        private float _localPositionY;

        private readonly PaintFloor _paintFloor;

        public HouseBuilder(int houseTypeNumber, List<Material> glassFloor, List<Material> simpleFloor)
        {
            _houseTypeNumber = houseTypeNumber;
            _simpleFloor = Resources.Load<GameObject>($"Buildings/House{houseTypeNumber}Type/House{houseTypeNumber}Floor");
            _glassFloor = Resources.Load<GameObject>("Buildings/GlassBuilding/GlassFloor");
            _roof = Resources.Load<GameObject>($"Buildings/House{houseTypeNumber}Type/House{houseTypeNumber}Roof");
            _glassRoof = Resources.Load<GameObject>("Buildings/GlassBuilding/GlassRoof");
            _paintFloor = new PaintFloor(glassFloor, simpleFloor);
        }

        public void ResetHouse()
        {
            _localPositionY = 0f;
            _floorNumber = 0;
            _houseNumber++;
            _house = new GameObject($"{HouseName}{_houseTypeNumber} {_houseNumber}");
        }

        public void CreateSimpleFloor()
        {
            CreateFloor(FloorType.SimpleFloor);
        }

        public void CreateGlassFloor()
        {
            CreateFloor(FloorType.GlassFloor);
        }

        public void CreateRoof()
        {
            CreateRoof(FloorType.SimpleFloor);
        }

        public void CreateGlassRoof()
        {
            CreateRoof(FloorType.GlassFloor);
        }

        private void CreateFloor(FloorType floorType)
        {
            GameObject floor;
            switch (floorType)
            {
                case FloorType.SimpleFloor:
                    floor = UnityEngine.Object.Instantiate(_simpleFloor, _house.transform, false);
                    _paintFloor.Paint(floor, false);
                    break;
                case FloorType.GlassFloor:
                    floor = UnityEngine.Object.Instantiate(_glassFloor, _house.transform, false);
                    _paintFloor.Paint(floor, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floorType), floorType, null);
            }
            CreateBuildingObject(floor);
        }

        private void CreateRoof(FloorType floorType)
        {
            GameObject floor;
            switch (floorType)
            {
                case FloorType.SimpleFloor:
                    floor = UnityEngine.Object.Instantiate(_roof, _house.transform, false);
                    _paintFloor.Paint(floor, false);
                    break;
                case FloorType.GlassFloor:
                    floor = UnityEngine.Object.Instantiate(_glassRoof, _house.transform, false);
                    _paintFloor.Paint(floor, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floorType), floorType, null);
            }
            CreateBuildingObject(floor);
        }

        private void CreateBuildingObject(GameObject floor)
        {
            floor.name = FloorName + _floorNumber;
            floor.GetComponent<FloorView>().SetFloorNumber(_floorNumber);
            _floorNumber++;
            var sizeY = floor.GetComponentInChildren<BoxCollider>().bounds.size.y;
            var position = new Vector3(0, _localPositionY + sizeY / 2, 0);
            floor.transform.localPosition = position;
            var rotation = Quaternion.Euler(0f, 0f, 0f);
            floor.transform.localRotation = rotation;
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