using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
	public class FoxAudioManager : MonoBehaviour, IFoxAudioManager
	{
		[SerializeField] private MainAudioCase _audioCase;
		[SerializeField] private AudioMixerSettingsPanel _audioMixer;

		private AudioGenerator _audioGenerator;

		private AudioObjectPool _audioObject;

		public AudioMixerSettingsPanel Mixer => _audioMixer;

		public static FoxAudioManager Instance { get; private set; }

		private FoxAudioManagerDataPool _dataPool;

		public void Initialization()
		{
			if(Instance != null && Instance != this)
				Destroy(gameObject);

			Instance = this;

			DontDestroyOnLoad(gameObject);
			_dataPool = new FoxAudioManagerDataPool();
			_audioGenerator = new AudioGenerator(_audioCase);
			_audioObject = new AudioObjectPool();
		}

		public void OnStart() =>
			_audioMixer.Initialization();

		public bool StopAudio(ControlledAudioResource controlledAudioResource)
		{
			if(!_dataPool.TryGetControlledAudioResource(controlledAudioResource))
				return false;
			
			controlledAudioResource.Audio.Stop();
			return true;
		}

		public void StopAllAudio()
		{
			List<ControlledAudioResource> resources =_dataPool.GetAll();

			foreach(ControlledAudioResource resource in resources)
				resource.Audio.Stop();
		}

		public bool PlayAudio(string key, Vector3 spawnPosition, out ControlledAudioResource controlledAudioResource) =>
			Play(key, spawnPosition,  out controlledAudioResource);

		public bool PlayAudioFollowingTarget(string key, Transform target, out ControlledAudioResource controlledAudioResource)
		{
			bool result = Play(key, target.position,  out controlledAudioResource);
			controlledAudioResource.Audio.SetTarget(target);
			return result;
		}

		private bool Play(string key, Vector3 spawnPosition, out ControlledAudioResource resource)
		{
			if(!TryPlayAudio(key, out resource))
				return false;

			resource.Audio.SetPosition(spawnPosition);
			resource.Audio.Name = key;
			resource.Audio.End += AudioOnEnd;

			_dataPool.Add(resource);

			resource.Audio.GameObject.SetActive(true);
			resource.Audio.Play();
			return true;
		}

		private void AudioOnEnd(IAudio audio)
		{
			audio.End -= AudioOnEnd;
			_dataPool.Get(audio);

			if(audio.GameObject == null)
				return;

			audio.GameObject.SetActive(false);
			_audioObject.Add(audio.Type, audio);
		}

		private bool TryPlayAudio(string key, out ControlledAudioResource iAudio)
		{
			iAudio = null;

			if(!_audioCase.TryGetAudioCase(key, out IAudioCase audioCase))
				return false;

			if(TryPlay<PlayListAudioCase, PlaylistData, Playlist>(audioCase, out iAudio))
				return true;
			if(TryPlay<SoloAudioCase, AudioData, Audio>(audioCase, out iAudio))
				return true;
			if(TryPlay<RandomAudioCase, RandomAudioData, RandomAudio>(audioCase, out iAudio))
				return true;

			return false;
		}

		private bool TryPlay<TAudioCase, TAudioDataBase, TAudioBase>(IAudioCase audioCase, out ControlledAudioResource iAudio)
			where TAudioDataBase : AudioDataBase
			where TAudioCase : AudioCase<TAudioDataBase>
			where TAudioBase : AudioBase<TAudioDataBase>, new()
		{
			iAudio = null;

			if(!(audioCase is TAudioCase tAudioCase))
				return false;

			TAudioBase audio = new TAudioBase();

			if(!_audioObject.Get(ref audio))
			{
				_audioGenerator.Generate(ref audio);

				if(audio is IAudio setIDAudio)
					setIDAudio.ID = $"{typeof(IAudio)}_{Time.time}_{Guid.NewGuid()}";

				audio.GameObject.transform.SetParent(transform);
			}

			audio.name = tAudioCase.Key;
			audio.Initialization(tAudioCase.AudioData);
			audio.Type = typeof(TAudioBase);
			iAudio = new ControlledAudioResource(audio);
			return true;
		}
	}


}