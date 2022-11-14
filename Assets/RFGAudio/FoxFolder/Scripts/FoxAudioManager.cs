using System;
using UnityEngine;

namespace RFG.Audio
{
	public class FoxAudioManager : MonoBehaviour
	{
		[SerializeField] private MainAudioCase _audioCase;
		private AudioGenerator _audioGenerator;
		private AudioObjectPool _audioObject;

		private static FoxAudioManager _instance = null; 
		
		private void Awake()
		{
			Initialization();
		}

		public void Initialization()
		{
			if(_instance !=null && _instance != this)
				Destroy(gameObject);

			_instance = this;
			DontDestroyOnLoad(gameObject);
			_audioGenerator = new AudioGenerator(_audioCase);
			_audioObject = new AudioObjectPool();
		}

		private void Start()
		{
			PlayAudioFollowingTarget("SoundTrack", Camera.main.transform, true);
			PlayAudio("Warp", transform.position, true);
		}

		public bool PlayAudioFollowingTarget(string key, Transform target, bool persist = false)
		{
			if(!TryPlayAudio(key, persist, out IAudio iAudio))
				return false;
			
			iAudio.SetPosition(target.position);
			iAudio.SetTarget(target);
			iAudio.Play();
			return true;
		}
		
		public bool PlayAudio(string key, Vector3 spawnPosition, bool persist = false)
		{
			if(!TryPlayAudio(key, persist, out IAudio iAudio))
				return false;
			
			iAudio.SetPosition(spawnPosition);
			iAudio.Play();
			return true;
		}
		
		private bool TryPlayAudio(string key, bool persist, out IAudio iAudio)
		{
			iAudio = null;
			
			if(!_audioCase.TryGetAudioCase(key, out IAudioCase audioCase))
				return false;
			
			if(TryPlay<PlayListAudioCase, PlaylistData, Playlist>(persist, audioCase, out iAudio))
				return true;
			if(TryPlay<SoloAudioCase, AudioData, Audio>(persist, audioCase, out iAudio))
				return true;
			if(TryPlay<RandomAudioCase, RandomAudioData, RandomAudio>(persist, audioCase, out iAudio))
				return true;
			
			return false;
		}
		
		private bool TryPlay<TAudioCase, TAudioDataBase, TAudioBase>(bool persist, IAudioCase audioCase, out IAudio iAudio) 
			where TAudioCase : AudioCase<TAudioDataBase> 
			where TAudioDataBase : AudioDataBase  
			where TAudioBase : AudioBase<TAudioDataBase> , new()
		{
			iAudio = null;
			
			if(audioCase is not TAudioCase tAudioCase)
				return false;
			
			TAudioBase audio = new ();

			if(!_audioObject.Get(ref audio))
				_audioGenerator.Generate(ref audio);

			audio.Initialization(tAudioCase.AudioData, persist);
			
			iAudio = audio;
			return true;
		}
	}
}