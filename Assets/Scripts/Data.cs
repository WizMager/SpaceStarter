using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Data", fileName = "Data")]
public class Data : ScriptableObject
{
    [SerializeField] private string _playerDataPath;
    [SerializeField] private string _planetDataPath;
    private PlayerData _playerData;
    private PlanetData _planetData;

    public PlayerData Player
    {
        get
        {
            if (_playerData == null)
            {
                _playerData = Load<PlayerData>(_playerDataPath);
            }

            return _playerData;
        }
    }

    public PlanetData Planet
    {
        get
        {
            if (_planetData == null)
            {
                _planetData = Load<PlanetData>(_planetDataPath);
            }

            return _planetData;
        }
    }

    private T Load<T>(string resourcePath) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(resourcePath, null));
    }
}