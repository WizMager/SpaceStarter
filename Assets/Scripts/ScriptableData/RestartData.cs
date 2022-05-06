using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/RestartData", fileName = "RestartData")]
    public class RestartData : ScriptableObject
    {
        public float waitAfterRestart;
    }
}