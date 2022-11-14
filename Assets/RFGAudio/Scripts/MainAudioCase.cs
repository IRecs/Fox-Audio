using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
	[CreateAssetMenu(menuName = "Create MainAudioCase", fileName = "MainAudioCase", order = 0)]
	public class MainAudioCase : ScriptableObject
	{
		[SerializeField] private List<SoloAudioCase> _soloAudio;
		[SerializeField] private List<PlayListAudioCase> _playListAudio;
		[SerializeField] private List<RandomAudioCase> _randomAudio;

		[SerializeField] private Audio _audioPrefab;
		[SerializeField] private Playlist _playlistPrefab;
		[SerializeField] private RandomAudio _randomAudioPrefab;

		public Audio AudioPrefab => _audioPrefab;
		public Playlist PlaylistPrefab => _playlistPrefab;
		public RandomAudio RandomAudioPrefab => _randomAudioPrefab;

		private Dictionary<string, IAudioCase> _cases;
		private Dictionary<Type, GameObject> _prefabs;

		public void Initialization()
		{
			_cases = new Dictionary<string, IAudioCase>();

			foreach(SoloAudioCase soloAudio in _soloAudio)
				_cases.Add(soloAudio.Key, soloAudio);

			foreach(PlayListAudioCase playListAudio in _playListAudio)
				_cases.Add(playListAudio.Key, playListAudio);

			foreach(RandomAudioCase randomAudio in _randomAudio)
				_cases.Add(randomAudio.Key, randomAudio);

			_prefabs = new Dictionary<Type, GameObject>();
			_prefabs.Add(_audioPrefab.GetType(), _audioPrefab.gameObject);
			_prefabs.Add(_playlistPrefab.GetType(), _playlistPrefab.gameObject);
			_prefabs.Add(_randomAudioPrefab.GetType(), _randomAudioPrefab.gameObject);
		}

		public bool TryGetAudioCase(string key, out IAudioCase audioCase)
		{
			audioCase = null;

			if(!_cases.ContainsKey(key))
				return false;

			audioCase = _cases[key];
			return true;
		}

		public bool GetAudioPrefab(Type type, ref GameObject audio)
		{
			if(!_prefabs.ContainsKey(type))
				return false;

			audio = _prefabs[type];
			return true;
		}

		private void OnValidate()
		{
			foreach(SoloAudioCase soloAudio in _soloAudio)
				soloAudio.Name = soloAudio.Key;

			foreach(PlayListAudioCase playListAudio in _playListAudio)
				playListAudio.Name = playListAudio.Key;

			foreach(RandomAudioCase randomAudio in _randomAudio)
				randomAudio.Name = randomAudio.Key;
		}
	}


	[Serializable]
	public class SoloAudioCase : AudioCase<AudioData>
	{
	}

	[Serializable]
	public class PlayListAudioCase : AudioCase<PlaylistData>
	{
	}

	[Serializable]
	public class RandomAudioCase : AudioCase<RandomAudioData>
	{
	}

	[Serializable]
	public class AudioCase<T> : IAudioCase where T : AudioDataBase
	{
		[HideInInspector] public string Name;
		public string Key => AudioData.name;
		public T AudioData;
	}

	public interface IAudioCase
	{
	}
}