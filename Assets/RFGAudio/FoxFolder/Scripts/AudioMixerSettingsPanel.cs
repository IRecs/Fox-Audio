using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
	public class AudioMixerSettingsPanel : MonoBehaviour
	{
		[SerializeField] private List<AudioMixerSettings> _audioMixerSettings;
    
		private void Start()
		{
			foreach (AudioMixerSettings settings in _audioMixerSettings)
				settings.Initialize();
		}
	}
}