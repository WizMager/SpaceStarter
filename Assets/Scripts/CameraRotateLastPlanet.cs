using UnityEngine;

public class CameraRotateLastPlanet 
{
	//ToDo: Relocate class to CameraMove
	//ToDo: Relocate class field here(camera and speed)
	public CameraRotateLastPlanet()
	{
		//ToDo: Initialize fields here
	}
	
	//TODO: pass deltaTime parameter in method
	public void CameraRotateTransform()
	{
		float speedRotate = 200;
		Transform camera = Camera.main.transform;

		//ToDo: Position of center planet add in field class and initialize it in constructor
		camera.RotateAround(GameObject.Find("LastPlanetObject").transform.position, camera.up,
			speedRotate * Time.deltaTime);
	}
}
