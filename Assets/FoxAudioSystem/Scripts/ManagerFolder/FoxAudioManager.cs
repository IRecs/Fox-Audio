using FoxAudioSystem.Scripts.CoreFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
	public class FoxAudioManager : MonoBehaviour, IFoxAudioManager
	{
		[SerializeField] private MainAudioCase _audioCase;
		[SerializeField] private AudioMixerSettingsPanel _audioMixer;

		private PlayAudioLogics _playAudioLogics;
		private FoxAudioManagerDataPool _dataPool;

		public static FoxAudioManager Instance { get; private set; }
		
		public AudioMixerSettingsPanel Mixer => _audioMixer;

		public void Initialization()
		{
			if(Instance != null && Instance != this)
				Destroy(gameObject);

			Instance = this;

			DontDestroyOnLoad(gameObject);
			_dataPool = new FoxAudioManagerDataPool();
			_playAudioLogics = new PlayAudioLogics(_dataPool, _audioCase, transform);
		}

		public void OnStart() =>
			_audioMixer.Initialization();

		public bool PlayAudio(string key, Vector3 spawnPosition, out ControlledAudioResource controlledAudioResource) =>
			_playAudioLogics.Play(key, spawnPosition, out controlledAudioResource);

		public bool PlayAudioFollowingTarget(string key, Transform target, out ControlledAudioResource controlledAudioResource)
		{
			bool result = _playAudioLogics.Play(key, target.position, out controlledAudioResource);
			controlledAudioResource.audioPlayer.SetTarget(target);
			return result;
		}
		
		public bool StopAudio(ControlledAudioResource controlledAudioResource)
		{
			if(!_dataPool.TryGetControlledAudioResource(controlledAudioResource))
				return false;

			controlledAudioResource.audioPlayer.Stop();
			return true;
		}

		public void StopAllAudio()
		{
			foreach(ControlledAudioResource resource in _dataPool.GetAll())
				resource.audioPlayer.Stop();
		}
	}
}