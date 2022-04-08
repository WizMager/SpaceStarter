using UnityEngine;

namespace Assets.Scripts.ScriptableData
{
	[CreateAssetMenu(menuName = "Data/SpeedRotateData", fileName ="SpeedRotateData")]
	public class SpeedRotateCameraData : ScriptableObject
	{
		public float speedRotate;
	}
}
