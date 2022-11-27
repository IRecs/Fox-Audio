using System.Collections.Generic;
using System.Linq;
using FoxAudioSystem.Scripts.DataFolder;
using NaughtyAttributes;
using UnityEngine;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
	[CreateAssetMenu(menuName = "SubCaseAudioGroup", fileName = "FoxAudioSystem//SubCaseAudioGroup", order = 0)]
	public class SubCaseAudioGroup : ScriptableObject, ICase
	{
		[BoxGroup("Main Group"), SerializeField]
		private List<SoloAudioCase> _soloAudio;
		[BoxGroup("Main Group"), SerializeField]
		private List<PlayListAudioCase> _playListAudio;
		[BoxGroup("Main Group"), SerializeField]
		private List<RandomAudioCase> _randomAudio;

		public void InitializationCases(ref Dictionary<string, IAudioCase> cases)
		{
			foreach(SoloAudioCase soloAudio in _soloAudio.Where(s => !s.IsNull))
				cases.Add(soloAudio.Key, soloAudio);

			foreach(PlayListAudioCase playListAudio in _playListAudio.Where(p => !p.IsNull))
				cases.Add(playListAudio.Key, playListAudio);

			foreach(RandomAudioCase randomAudio in _randomAudio.Where(r => !r.IsNull))
				cases.Add(randomAudio.Key, randomAudio);
		}

		public void Add<T>(T newAsset) where T : ScriptableObject
		{
			if(newAsset is SoloAudioClipData soloAudioClipData)
				_soloAudio.Add(new SoloAudioCase() { AudioData = soloAudioClipData });
			else if(newAsset is PlaylistDataCase playListAudioCase)
				_playListAudio.Add(new PlayListAudioCase() { AudioData = playListAudioCase });
			else if(newAsset is RandomAudioDataCase randomAudioCase)
				_randomAudio.Add(new RandomAudioCase() { AudioData = randomAudioCase });

			OnValidate();
		}

		private void OnValidate()
		{
			foreach(SoloAudioCase soloAudio in _soloAudio.Where(p => !p.IsNull))
				soloAudio.Name = soloAudio.Key;

			foreach(PlayListAudioCase playListAudio in _playListAudio.Where(p => !p.IsNull))
				playListAudio.Name = playListAudio.Key;

			foreach(RandomAudioCase randomAudio in _randomAudio.Where(p => !p.IsNull))
				randomAudio.Name = randomAudio.Key;
		}

		[Button("Sort by Name")]
		private void Sort()
		{
			_soloAudio = _soloAudio.Where(p => !p.IsNull).OrderBy(p => p.Name).ToList();
			_playListAudio = _playListAudio.Where(p => !p.IsNull).OrderBy(p => p.Name).ToList();
			_randomAudio = _randomAudio.Where(p => !p.IsNull).OrderBy(p => p.Name).ToList();
		}
	}
}