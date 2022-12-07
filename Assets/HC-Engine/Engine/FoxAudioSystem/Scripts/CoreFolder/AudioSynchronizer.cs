using System;
using System.Collections.Generic;
using FoxAudioSystem.Scripts.PlayersFolder;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class AudioSynchronizer
	{
		public string MainKey;
		private readonly int _maxSound;

		private readonly List<IAudioPlayer> _audios;
		private IAudioPlayer  _mainAudio;

		public AudioSynchronizer(string mainKey, int maxSound)
		{
			MainKey = mainKey;
			_maxSound = maxSound;
			_audios = new List<IAudioPlayer>();
		}

		public void AddAudio(IAudioPlayer  audio)
		{
			if(_audios.Contains(audio))
				return;

			if(_audios.Count == 0)
				_mainAudio = audio;
			audio.End += AudioOnEnd;
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
					_audios[i].SynchronizeLogic.Synchronize(_mainAudio.GetData(), _mainAudio.SynchronizeLogic.TimeSamples, volume);
				else
					_audios[i].SynchronizeLogic.Synchronize(_mainAudio.GetData(), _mainAudio.SynchronizeLogic.TimeSamples, 0);
			}
		}

		private void AudioOnEnd(IAudioPlayer audioPlayer)
		{
			_audios.Remove((SoloAudioPlayer)audioPlayer);

			if(_mainAudio.Equals(audioPlayer) && _audios.Count > 0)
				_mainAudio = _audios[0];

			SynchronizeVolume();
		}
	}

	public static class AudioSynchronizerCase
	{
		private static Dictionary<string, AudioSynchronizer> _synchronizers = new Dictionary<string, AudioSynchronizer>();

		public static void Add(IAudioPlayer audio)
		{
			if(!_synchronizers.ContainsKey(audio.Name))
				_synchronizers.Add(audio.Name, new AudioSynchronizer(audio.Name, 3));
			
			_synchronizers[audio.Name].AddAudio(audio);
		}
	}
}