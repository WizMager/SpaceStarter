using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(menuName = "Data/InputData", fileName = "InputData")]
    public class InputData : ScriptableObject
    {
        public float minimalDistanceForSwipe;
    }
}