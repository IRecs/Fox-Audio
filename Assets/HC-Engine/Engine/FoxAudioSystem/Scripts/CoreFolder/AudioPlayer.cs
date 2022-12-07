using System.Collections.Generic;
using Engine.DI;
using FoxAudioSystem.Scripts.ManagerFolder;
using FoxAudioSystem.Scripts.PlayersFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class AudioPlayer
	{
		private readonly IFoxAudioManager _foxAudioManager;
		private readonly Dictionary<string, List<ControlledAudioResource>> _controlledAudio;
		
		public AudioPlayer()
		{
			_foxAudioManager = DIContainer.AsSingle<IFoxAudioManager>();
			_controlledAudio = new Dictionary<string, List<ControlledAudioResource>>();
		}

		/// <summary>
		/// Launches static audio resources at the specified location
		/// </summary>
		/// <param name="key">Name Audio Resource</param>
		/// <param name="position">The position where audio playback starts</param>
		public void Play(string key, Vector3 position)
		{
			if(_foxAudioManager.PlayAudio(key, position, out ControlledAudioResource controlledAudioResource))
				Add(controlledAudioResource);
		}

		/// <summary>
		/// Follows the specified target but is not its child
		/// </summary>
		/// <param name="key">Name Audio Resource</param>
		/// <param name="target">Target to be followed by audio</param>
		public void Play(string key, Transform target)
		{
			if(_foxAudioManager.PlayAudioFollowingTarget(key, target, out ControlledAudioResource controlledAudioResource))
				Add(controlledAudioResource);
		}

		private void Add(ControlledAudioResource controlledAudioResource)
		{
			if(!_controlledAudio.ContainsKey(controlledAudioResource.Key))
				_controlledAudio.Add(controlledAudioResource.Key, new List<ControlledAudioResource>(1));

			_controlledAudio[controlledAudioResource.Key].Add(controlledAudioResource);
			controlledAudioResource.audioPlayer.End += AudioPlayerOnEnd;
		}

		private void AudioPlayerOnEnd(IAudioPlayer audioPlayer)
		{
			ControlledAudioResource controlledAudioResource = _controlledAudio[audioPlayer.Name].Find(c => c.ID == audioPlayer.ID);

			if(controlledAudioResource == null)
				return;

			controlledAudioResource.audioPlayer.End -= AudioPlayerOnEnd;
			_controlledAudio[audioPlayer.Name].Remove(controlledAudioResource);
		}
		
		/// <summary>
		/// Stops audio playback if such an object plays audio with such a key
		/// </summary>
		/// <param name="key">Name Audio Resource</param>
		public void Stop(string key)
		{
			if(!_controlledAudio.ContainsKey(key) || _controlledAudio[key].Count == 0)
				return;

			_foxAudioManager.StopAudio(_controlledAudio[key][0]);
		}
	}
}