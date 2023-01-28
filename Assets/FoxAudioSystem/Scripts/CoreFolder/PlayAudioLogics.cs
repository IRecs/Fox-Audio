using System;
using System.Collections.Generic;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ManagerFolder;
using FoxAudioSystem.Scripts.PlayersFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class PlayAudioLogics
	{
		private readonly FoxAudioManagerDataPool _dataPool;
		private readonly MainAudioCase _audioCase;
		private readonly AudioObjectPool _audioObject;
		private readonly AudioGenerator _audioGenerator;
		private readonly Transform _transform;

		private Dictionary<string, AudioSynchronizer> _synchronizers = new Dictionary<string, AudioSynchronizer>();

		public PlayAudioLogics(FoxAudioManagerDataPool dataPool, MainAudioCase audioCase, Transform transform)
		{
			_dataPool = dataPool;
			_audioCase = audioCase;
			_audioObject = new AudioObjectPool();
			_audioGenerator = new AudioGenerator(audioCase);
			_transform = transform;
		}

		public bool Play(string key, Vector3 spawnPosition, out ControlledAudioResource resource)
		{
			if(!TryPlayAudio(key, out resource))
				return false;

			resource.audioPlayer.SetPosition(spawnPosition);
			resource.audioPlayer.End += AudioPlayerOnEnd;

			_dataPool.Add(resource);

			resource.audioPlayer.GameObject.SetActive(true);
			resource.audioPlayer.Play();
			return true;
		}

		private bool TryPlayAudio(string key, out ControlledAudioResource iAudio)
		{
			iAudio = null;

			if(!_audioCase.TryGetAudioCase(key, out IAudioCase audioCase))
				return false;

			if(TryPlay<PlayListAudioCase, PlaylistDataCase, PlaylistPlayer>(audioCase, out iAudio))
				return true;
			if(TryPlay<SoloAudioCase, SoloAudioClipData, SoloAudioPlayer>(audioCase, out iAudio))
				return true;
			if(TryPlay<RandomAudioCase, RandomAudioDataCase, RandomAudioPlayer>(audioCase, out iAudio))
				return true;

			return false;
		}

		private bool TryPlay<TAudioCase, TAudioDataBase, TAudioBase>(IAudioCase audioCase, out ControlledAudioResource iAudio)
			where TAudioDataBase : AudioDataBase
			where TAudioCase : AudioCase<TAudioDataBase>
			where TAudioBase : AudioPlayerBase<TAudioDataBase>, new()
		{
			iAudio = null;

			if(!(audioCase is TAudioCase tAudioCase))
				return false;
			
#pragma warning disable 
			TAudioBase audio = new TAudioBase();
#pragma warning restore
			
			if(!_audioObject.Get(ref audio))
			{
				_audioGenerator.Generate(ref audio);

				if(audio is IAudioPlayer setIDAudio)
					setIDAudio.ID = $"{typeof(IAudioPlayer)}_{Time.time}_{Guid.NewGuid()}";

				audio.GameObject.transform.SetParent(_transform);
			}

			audio.Initialization(tAudioCase.AudioData, tAudioCase.Key);
			
			audio.Type = typeof(TAudioBase);
			iAudio = new ControlledAudioResource(audio);
			return true;
		}

		private void AudioPlayerOnEnd(IAudioPlayer audioPlayer)
		{
			audioPlayer.End -= AudioPlayerOnEnd;
			_dataPool.Get(audioPlayer);

			if(audioPlayer.GameObject == null)
				return;

			audioPlayer.GameObject.SetActive(false);
			_audioObject.Add(audioPlayer.Type, audioPlayer);
		}
	}
}