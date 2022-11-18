using UnityEngine;

namespace RFG.Audio
{
	public interface IFoxAudioManager
	{
		bool StopAudio(ControlledAudioResource controlledAudioResource);
		bool PlayAudioFollowingTarget(string key, Transform target, out ControlledAudioResource controlledAudioResource);
		bool PlayAudio(string key, Vector3 spawnPosition, out ControlledAudioResource controlledAudioResource);
		void StopAllAudio();
		AudioMixerSettingsPanel Mixer { get; }
	}
}