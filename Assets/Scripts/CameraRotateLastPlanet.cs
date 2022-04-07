using UnityEngine;

public class CameraRotateLastPlanet 
{
	public void CameraRotateTransform()
	{
		float speedRotate = 200;
		Transform camera = Camera.main.transform;

		camera.RotateAround(GameObject.Find("LastPlanetObject").transform.position, camera.up,
			speedRotate * Time.deltaTime);
	}
}
