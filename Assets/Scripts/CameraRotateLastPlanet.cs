using Assets.Scripts.ScriptableData;
using ScriptableData;
using UnityEngine;

public class CameraRotateLastPlanet 
{
	Transform _camera;
	float _speedRotate;
	Vector3 _centerPoint;

	public CameraRotateLastPlanet(float speedDrift, Transform camera, Vector3 centerPoint)
	{
		_camera = camera;
		_centerPoint = centerPoint;
		_speedRotate = speedDrift;
	}

	public bool CameraRotateTransform(float deltaTime)
	{
		_camera.RotateAround(_centerPoint, _camera.up,
			_speedRotate * deltaTime);

		return true;
	}
}
