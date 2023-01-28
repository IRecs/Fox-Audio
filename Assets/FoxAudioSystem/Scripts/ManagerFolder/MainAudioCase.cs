using System;
using System.Collections.Generic;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.PlayersFolder;
using NaughtyAttributes;
using UnityEngine;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
	[CreateAssetMenu(menuName = "FoxAudioSystem/MainAudioCase", fileName = "MainAudioCase", order = 0)]
	public class MainAudioCase : SubCaseAudioGroup
	{
		[Label("Cases")]
		[BoxGroup("Subgroup"), SerializeField] private List<SubCaseAudioGroup> _subCaseAudioGroups;

		[BoxGroup("Prefabs"), SerializeField] private bool _advancedSettings;
		[BoxGroup("Prefabs"), SerializeField, ShowIf(nameof(_advancedSettings))]
		private SoloAudioPlayer SoloAudioPlayerPrefab;
		[BoxGroup("Prefabs"), SerializeField, ShowIf(nameof(_advancedSettings))]
		private PlaylistPlayer PlaylistPlayerPrefab;
		[BoxGroup("Prefabs"), SerializeField, ShowIf(nameof(_advancedSettings))]
		private RandomAudioPlayer RandomAudioPlayerPrefab;

		private Dictionary<string, IAudioCase> _cases;
		private Dictionary<Type, GameObject> _prefabs;

		public void Initialization()
		{
			_cases = GetCase();

			_prefabs = new Dictionary<Type, GameObject>();
			_prefabs.Add(SoloAudioPlayerPrefab.GetType(), SoloAudioPlayerPrefab.gameObject);
			_prefabs.Add(PlaylistPlayerPrefab.GetType(), PlaylistPlayerPrefab.gameObject);
			_prefabs.Add(RandomAudioPlayerPrefab.GetType(), RandomAudioPlayerPrefab.gameObject);
		}

		private Dictionary<string, IAudioCase> GetCase()
		{
			Dictionary<string, IAudioCase> cases = new Dictionary<string, IAudioCase>();

			InitializationCases(ref _cases);
			
			foreach(SubCaseAudioGroup subCaseAudioGroup in _subCaseAudioGroups)
				subCaseAudioGroup.InitializationCases(ref cases);

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