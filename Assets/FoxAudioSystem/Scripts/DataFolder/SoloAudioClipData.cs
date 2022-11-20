using UnityEngine;

namespace FoxAudioSystem.Scripts.DataFolder
{
	[CreateAssetMenu(fileName = "Solo Audio Clip Data", menuName = "FoxAudioSystem/Audio/Data/Solo/Solo Audio Clip Data")]
	public class SoloAudioClipData : AudioData
	{
		public bool Synchronize;
	}
}