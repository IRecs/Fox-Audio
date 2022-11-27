using System;
using System.Collections.Generic;
using FoxAudioSystem.Scripts.PlayersFolder;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class AudioSynchronizer
	{
		public string MainKey;
		private readonly int _maxSound;

		private List<SoloAudioPlayer > _audios;
		private SoloAudioPlayer  _mainAudio;

		public AudioSynchronizer(string mainKey, int maxSound)
		{
			MainKey = mainKey;
			_maxSound = maxSound;
			_audios = new List<SoloAudioPlayer >();
		}

		public void AddAudio(SoloAudioPlayer  audio)
		{
			if(_audios.Contains(audio))
				return;

			if(_audios.Count == 0)
				_mainAudio = audio;
			audio.End += AudioOnEnd;
			audio.Synchronize(_mainAudio.TimeSamples);
			_audios.Add(audio);
			SynchronizeVolume();
		}
		
		private void SynchronizeVolume()
		{
			if(_audios.Count == 0)
				return;
			
			float volume = 1f / Math.Clamp(_audios.Count, 1, _maxSound);
			
			for( int i = 0; i < _audios.Count; i++ )
			{
				if(i < _maxSound)
					_audios[i].SynchronizeVolume(volume);
				else
					_audios[i].SynchronizeVolume(0);
			}
		}

		private void AudioOnEnd(IAudio audio)
		{
			_audios.Remove((SoloAudioPlayer)audio);

			if(_mainAudio.Equals(audio) && _audios.Count > 0)
				_mainAudio = _audios[0];

			SynchronizeVolume();
		}
	}

	public static class AudioSynchronizerCase
	{
		private static Dictionary<string, AudioSynchronizer> _synchronizers = new Dictionary<string, AudioSynchronizer>();

		public static void Add(SoloAudioPlayer  audio)
		{
			if(!_synchronizers.ContainsKey(audio.Name))
				_synchronizers.Add(audio.Name, new AudioSynchronizer(audio.Name, 3));
			
			_synchronizers[audio.Name].AddAudio(audio);
		}
	}
}