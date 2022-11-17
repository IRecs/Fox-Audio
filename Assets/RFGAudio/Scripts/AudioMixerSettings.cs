using System;
using UnityEngine;
using UnityEngine.Audio;

namespace RFG.Audio
{
	[CreateAssetMenu(fileName = "New Audio Mixer Settings", menuName = "RFG/Audio/Audio Mixer Settings")]
	public class AudioMixerSettings : ScriptableObject
	{
		[HideInInspector] public string Name;
		public AudioMixer AudioMixer;
		public float Volume;
		private string Key => $"{Name}_{nameof(AudioMixerSettings)}";
		private string KeyDefault => $"{Name}_{nameof(AudioMixerSettings)}_Default";

		public void Initialize()
		{
			Volume = PlayerPrefs.GetFloat(Key, Volume);
			SetVolume(Volume);
		}

		public void SetVolume(float volume)
		{
			volume = Mathf.Clamp(volume, 0.001f, 1);
			Volume = volume;
			AudioMixer.SetFloat("MainVolume", Mathf.Log(Volume) * 20);
			PlayerPrefs.SetFloat(Key, Volume);
		}

		public void Mute()
		{
			PlayerPrefs.SetFloat(KeyDefault, Volume);
			AudioMixer.SetFloat("MainVolume", -80f);
		}

		public void Unmute()
		{
			Volume = PlayerPrefs.GetFloat(KeyDefault, Volume);
			SetVolume(Volume);
		}

		public float GetVolume() =>
			Volume;

		private void OnValidate()
		{
			if(!name.Equals(Name))
				Name = name;
		}
	}
}