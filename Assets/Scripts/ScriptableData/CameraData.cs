﻿using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/CameraData", fileName = "CameraData")]
    public class CameraData : ScriptableObject
    {
        [Header("Camera Up")]
        public float upSpeed;
        public float upOffsetFromPlayer;
        public float moveSpeed;
        [Header("First Person(Last Planet)")]
        public float firstPersonRotationSpeed;
        [Header("Camera Down")]
        public float cameraDownPosition;
        public float cameraDownSpeed;
        [Header("Rotate Around Planet")] 
        public float cameraOffsetBeforeRotation;
        [Header("Fly from planet")]
        public float moveSpeedFlyFromPlanet;
        public float offsetBackFlyFromPlayer;
        public float offsetUpFlyFromPlayer;
        
        [Header("Fly radius")]
        public float rotationSpeedFlyRadius;
        public float moveSpeedFlyRadius;
        public float offsetBackFlyRadius;
        public float offsetUpFlyRadius;
        
        [Header("Fly camera down")]
        public float rotationSpeedFlyCameraDown;
        public float moveSpeedFlyCameraDown;
        public float offsetBackFlyCameraDown;
        public float offsetUpFlyCameraDown;
        
        [Header("Fly first person")]
        public float rotationSpeedFlyFirstPerson;
        public float moveSpeedFlyFirstPerson;
        public float offsetBackFlyFirstPerson;
        public float offsetUpFlyFirstPerson;

    }
}