using System.IO;
using UnityEngine;

public class Data : ScriptableObject
{
    [SerializeField] private string _playerDataPath;
    private PlayerData _playerData;

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

    private T Load<T>(string resourcePath) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(resourcePath, null));
    }
}