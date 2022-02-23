using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFiller : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectFiller;
    [SerializeField] private GameObject _emptyGameObject;
    
    private Vector3 _sphereScales;

    private void Start()
    {
        var gameObjectScales = gameObject.transform.localScale;
        _sphereScales.x = gameObjectScales.x;
        _sphereScales.y = gameObjectScales.y;
        _sphereScales.z = gameObjectScales.z;
        
        Instantiate(EmptyGOFiller(_emptyGameObject));
    }

    private GameObject EmptyGOFiller(GameObject emptyGo)
    {
        for (int x = 0; x < _sphereScales.x; x++)
        {
            Debug.Log(_sphereScales.x);
            for (int y = 0; y < _sphereScales.y; y++)
            {
                for (int z = 0; z < _sphereScales.z; z++)
                {
                    Debug.Log(_sphereScales.z);
                    Instantiate(_gameObjectFiller, Vector3FormScalesAndFiller(_sphereScales, _gameObjectFiller), Quaternion.identity)
                        .transform.SetParent(emptyGo.transform);
                    _sphereScales.z -= _gameObjectFiller.transform.localScale.z;
                }
                Debug.Log(_sphereScales.y);
                _sphereScales.y -= _gameObjectFiller.transform.localScale.y;
            }

            _sphereScales.x -= _gameObjectFiller.transform.localScale.x;
        }

        return _emptyGameObject;
    }

    private Vector3 Vector3FormScalesAndFiller(Vector3 scales, GameObject filler)
    {
        var fillerScales = filler.transform.localScale;
        var vector = new Vector3(scales.x - fillerScales.x / 2, 
            scales.y - fillerScales.y / 2,
            scales.z - fillerScales.z / 2);
        return vector;
    }
}

