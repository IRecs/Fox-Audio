using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
	public class FoxAudioManager : MonoBehaviour
	{
		[SerializeField] private MainAudioCase _audioCase;
		private AudioGenerator _audioGenerator;
		private AudioObjectPool _audioObject;

		private static FoxAudioManager _instance = null;

		private Dictionary<string, List<IAudio>> _playingAudio;

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
			_playingAudio = new Dictionary<string, List<IAudio>>();
			_audioGenerator = new AudioGenerator(_audioCase);
			_audioObject = new AudioObjectPool();
		}

		private void Start()
		{
			PlayAudioFollowingTarget("SoundTrack", Camera.main.transform, true);
			PlayAudio("Warp", transform.position, true);
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.G))
				PlayAudio("Warp", transform.position, true);
			
			if(Input.GetKeyDown(KeyCode.P))
				StopAudio("SoundTrack");
		}

		public bool StopAudio(string key)
		{
			if(!_playingAudio.ContainsKey(key))
				return false;
			if(_playingAudio[key].Count <= 0)
				return false;
			
			IAudio audio = _playingAudio[key][0];
			audio.Stop();

			return true;
		}
		
		public bool StopAllAudio(string key)
		{
			if(!_playingAudio.ContainsKey(key))
				return false;
			if(_playingAudio[key].Count <= 0)
				return false;

			foreach(IAudio audio in  _playingAudio[key])
				audio.Stop();

			return true;
		}
		
		public bool PlayAudioFollowingTarget(string key, Transform target, bool persist = false)
		{
			if(!PlayAudio(key,  target.position, out IAudio iAudio, persist))
				return false;
			
			iAudio.SetTarget(target);
			return true;
		}
		
		public bool PlayAudio(string key, Vector3 spawnPosition, bool persist = false) =>
			PlayAudio(key,  spawnPosition, out IAudio iAudio, persist);

		private bool PlayAudio(string key, Vector3 spawnPosition, out IAudio iAudio,bool persist = false)
		{
			if(!TryPlayAudio(key, persist, out iAudio))
				return false;
			
			iAudio.SetPosition(spawnPosition);
			iAudio.Name = key;
			iAudio.End += AudioOnEnd;
			
			if(!_playingAudio.ContainsKey(key))
				_playingAudio.Add(key, new List<IAudio>());
			
			_playingAudio[key].Add(iAudio);
			
			iAudio.GameObject.SetActive(true);
			iAudio.Play();
			return true;
		}
		
		private void AudioOnEnd(IAudio audio)
		{
			audio.End -= AudioOnEnd;
			_playingAudio[audio.Name].Remove(audio);
			audio.Persist(false);
			audio.GameObject.SetActive(false);
			audio.GameObject.transform.SetParent(transform);
			_audioObject.Add(audio.Type ,audio);
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
			where TAudioDataBase : AudioDataBase  
			where TAudioCase : AudioCase<TAudioDataBase> 
			where TAudioBase : AudioBase<TAudioDataBase> , new()
		{
			iAudio = null;
			
			if(audioCase is not TAudioCase tAudioCase)
				return false;
			
			TAudioBase audio = new ();

			if(!_audioObject.Get(ref audio))
				_audioGenerator.Generate(ref audio);
			audio.GameObject.transform.SetParent(null);
			audio.Initialization(tAudioCase.AudioData, persist);
			audio.Type = typeof(TAudioBase);
			iAudio = audio;
			return true;
		}
	}
}