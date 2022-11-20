using System;
using System.Collections.Generic;
using System.Linq;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.PlayersFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
	[CreateAssetMenu(menuName = "Create MainAudioCase", fileName = "MainAudioCase", order = 0)]
	public class MainAudioCase : ScriptableObject
	{
		[SerializeField] private List<SubCaseAudioGroup> _subCaseAudioGroups;

		[SerializeField] private List<SoloAudioCase> _soloAudio;
		[SerializeField] private List<PlayListAudioCase> _playListAudio;
		[SerializeField] private List<RandomAudioCase> _randomAudio;

		[SerializeField] private SoloAudioPlayer _soloAudioPlayerPrefab;
		[SerializeField] private PlaylistPLayer _playlistPLayerPrefab;
		[SerializeField] private RandomAudioPlayer _randomAudioPlayerPrefab;

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

			foreach(SubCaseAudioGroup subCaseAudioGroup in _subCaseAudioGroups)
				subCaseAudioGroup.Initialization(ref _cases);

			_prefabs = new Dictionary<Type, GameObject>();
			_prefabs.Add(_soloAudioPlayerPrefab.GetType(), _soloAudioPlayerPrefab.gameObject);
			_prefabs.Add(_playlistPLayerPrefab.GetType(), _playlistPLayerPrefab.gameObject);
			_prefabs.Add(_randomAudioPlayerPrefab.GetType(), _randomAudioPlayerPrefab.gameObject);
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

			_soloAudio = _soloAudio.OrderBy(p => p.Name).ToList();
			_playListAudio = _playListAudio.OrderBy(p => p.Name).ToList();
			_randomAudio = _randomAudio.OrderBy(p => p.Name).ToList();
		}
	}

	[Serializable]
	public class SoloAudioCase : AudioCase<SoloAudioClipData>
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