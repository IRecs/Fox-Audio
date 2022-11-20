using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
	[CreateAssetMenu(menuName = "SubCaseAudioGroup", fileName = "RFG/Audio/SubCaseAudioGroup", order = 0)]
	public class SubCaseAudioGroup : ScriptableObject
	{
		[SerializeField] private List<SoloAudioCase> _soloAudio;
		[SerializeField] private List<PlayListAudioCase> _playListAudio;
		[SerializeField] private List<RandomAudioCase> _randomAudio;

		public void Initialization(ref Dictionary<string, IAudioCase> cases)
		{
			foreach(SoloAudioCase soloAudio in _soloAudio)
				cases.Add(soloAudio.Key, soloAudio);

			foreach(PlayListAudioCase playListAudio in _playListAudio)
				cases.Add(playListAudio.Key, playListAudio);

			foreach(RandomAudioCase randomAudio in _randomAudio)
				cases.Add(randomAudio.Key, randomAudio);
		}

		private void OnValidate()
		{
			foreach(SoloAudioCase soloAudio in _soloAudio)
				soloAudio.Name = soloAudio.Key;

			foreach(PlayListAudioCase playListAudio in _playListAudio)
				playListAudio.Name = playListAudio.Key;

			foreach(RandomAudioCase randomAudio in _randomAudio)
				randomAudio.Name = randomAudio.Key;
			
			_soloAudio =  _soloAudio.OrderBy(p => p.Name).ToList();
			_playListAudio =  _playListAudio.OrderBy(p => p.Name).ToList();
			_randomAudio =  _randomAudio.OrderBy(p => p.Name).ToList();
		}
	}
}