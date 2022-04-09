using Assets.Scripts.ScriptableData;
using ScriptableData;
using UnityEngine;

public class CameraRotateLastPlanet 
{
	private readonly Transform _camera;
	private readonly float _speedRotate;
	private readonly Vector3 _centerPoint;

	public CameraRotateLastPlanet(float speedDrift, Transform camera, Vector3 centerPoint)
	{
		_camera = camera;
		_centerPoint = centerPoint;
		_speedRotate = speedDrift;
	}

	public void CameraRotateTransform(float deltaTime)
	{
		_camera.RotateAround(_centerPoint, _camera.up,
			_speedRotate * deltaTime);
	}
}
