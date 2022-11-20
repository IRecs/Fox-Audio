using System;
using System.Collections.Generic;
using System.Linq;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.PlayersFolder;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
	[CreateAssetMenu(menuName = "Create MainAudioCase", fileName = "MainAudioCase", order = 0)]
	public class MainAudioCase : ScriptableObject
	{
		[BoxGroup("Main Group"), SerializeField]
		private List<SoloAudioCase> _soloAudio;
		[BoxGroup("Main Group"), SerializeField]
		private List<PlayListAudioCase> _playListAudio;
		[BoxGroup("Main Group"), SerializeField]
		private List<RandomAudioCase> _randomAudio;

		[Label("Cases")]
		[BoxGroup("Subgroup"), SerializeField] private List<SubCaseAudioGroup> _subCaseAudioGroups;

		[BoxGroup("Prefabs"), SerializeField] private bool _advancedSettings;
		[BoxGroup("Prefabs"), SerializeField, ShowIf(nameof(_advancedSettings))]
		private SoloAudioPlayer _soloAudioPlayerPrefab;
		[BoxGroup("Prefabs"), SerializeField, ShowIf(nameof(_advancedSettings))]
		private PlaylistPLayer _playlistPLayerPrefab;
		[BoxGroup("Prefabs"), SerializeField, ShowIf(nameof(_advancedSettings))]
		private RandomAudioPlayer _randomAudioPlayerPrefab;

		private Dictionary<string, IAudioCase> _cases;
		private Dictionary<Type, GameObject> _prefabs;

		public void Initialization()
		{
			_cases = GetCase();

			_prefabs = new Dictionary<Type, GameObject>();
			_prefabs.Add(_soloAudioPlayerPrefab.GetType(), _soloAudioPlayerPrefab.gameObject);
			_prefabs.Add(_playlistPLayerPrefab.GetType(), _playlistPLayerPrefab.gameObject);
			_prefabs.Add(_randomAudioPlayerPrefab.GetType(), _randomAudioPlayerPrefab.gameObject);
		}

		private Dictionary<string, IAudioCase> GetCase()
		{
			Dictionary<string, IAudioCase> cases = new Dictionary<string, IAudioCase>();

			foreach(SoloAudioCase soloAudio in _soloAudio.Where(s => !s.IsNull))
				cases.Add(soloAudio.Key, soloAudio);

			foreach(PlayListAudioCase playListAudio in _playListAudio.Where(p => !p.IsNull))
				cases.Add(playListAudio.Key, playListAudio);

			foreach(RandomAudioCase randomAudio in _randomAudio.Where(r => !r.IsNull))
				cases.Add(randomAudio.Key, randomAudio);

			foreach(SubCaseAudioGroup subCaseAudioGroup in _subCaseAudioGroups)
				subCaseAudioGroup.Initialization(ref cases);

			return cases;
		}

		[Button()]
		private void GenerateKeys()
		{
			Dictionary<string, IAudioCase> cases = GetCase();

			List<string> names = new List<string>(cases.Count);

			foreach(KeyValuePair<string, IAudioCase> t in cases)
				names.Add(t.Key);

			AudioKeyGenerator.Generate(names.ToArray());
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
	public class PlayListAudioCase : AudioCase<PlaylistDataCase>
	{
	}

	[Serializable]
	public class RandomAudioCase : AudioCase<RandomAudioDataCase>
	{
	}

	[Serializable]
	public class AudioCase<T> : IAudioCase where T : AudioDataBase
	{
		[HideInInspector] public string Name;
		public string Key => AudioData.name;
		public T AudioData;
		public bool IsNull => AudioData == null;
	}

	public interface IAudioCase
	{
	}

}