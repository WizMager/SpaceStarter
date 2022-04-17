using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    private List<Transform> _allObjectssTransforms = new List<Transform>();
    private List<Vector3> _allObjectPositions = new List<Vector3>();

       private void Awake()
       {
            TakeAllObjects();  
       }

       private void TakeAllObjects()
       {
           var objectNumber = 0;
           var transforms = gameObject.GetComponentsInChildren<Transform>();
           foreach (var currentTransform in transforms)
           {
               objectNumber++;
               _allObjectssTransforms.Add(currentTransform);
               _allObjectPositions.Add(currentTransform.position);
           }
           Debug.Log(objectNumber);
       }
}