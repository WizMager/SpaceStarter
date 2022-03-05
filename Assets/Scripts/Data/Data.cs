using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Data", fileName = "Data")]
public class Data : ScriptableObject
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private LastPlanetData lastPlanetData;
    //[SerializeField] private PlanetData _planetData;

    public PlayerData Player
    {
        get
        {
            if (_playerData == null)
            {
                throw new NullReferenceException("You don't create PlayerData scriptable object");
            }

            return _playerData;
        }
    }

    public LastPlanetData LastPlanet
    {
        get
        {
            if (lastPlanetData == null)
            {
                throw new NullReferenceException("You don't create LastPlanetData scriptable object");
            }

            return lastPlanetData;
        }
    }

    // public PlanetData Planet
    // {
    //     get
    //     {
    //         if (_planetData == null)
    //         {
    //             throw new NullReferenceException("You don't create PlanetData scriptable object");
    //         }
    //         
    //         return Planet;
    //     }
    // }
}