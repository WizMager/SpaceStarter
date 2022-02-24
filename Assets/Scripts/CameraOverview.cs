using System;
using UnityEngine;

public class CameraOverview : MonoBehaviour
{
      [SerializeField] private Transform _planetCenter;
      [SerializeField] private Camera _camera;
      [SerializeField] private float _rotationSensitive;
      
      private void Update()
      {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                  MoveHorizontal(Input.GetAxis("Horizontal"));
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                  MoveVertical(Input.GetAxis("Vertical"));
            }
      }

      private void MoveHorizontal(float axisHorizontal)
      {
            _camera.transform.RotateAround(_planetCenter.position, Vector3.up, -axisHorizontal * _rotationSensitive);
      }

      private void MoveVertical(float axisVertical)
      {
            _camera.transform.RotateAround(_planetCenter.position, Vector3.right, axisVertical * _rotationSensitive);
      }
}