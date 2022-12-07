using FoxAudioSystem.Scripts.DataFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
	public interface ISynchronizeLogic
	{
		void Synchronize(AudioData audioData, int timeSamples, float volumeMultiplier);
		int TimeSamples { get; }
	}
	
	public abstract class SynchronizeLogic : ISynchronizeLogic
	{
		private readonly AudioSource _audioSource;

		public int TimeSamples => _audioSource.timeSamples;

		public SynchronizeLogic(AudioSource audioSource)
		{
			_audioSource = audioSource;
		}

		public void Synchronize(AudioData audioData, int timeSamples, float volumeMultiplier)
		{
			OnStartSynchronize(audioData);

			if(_audioSource.timeSamples != timeSamples)
				_audioSource.timeSamples = timeSamples;

			_audioSource.volume = audioData.volume * volumeMultiplier;
		}

		protected abstract void OnStartSynchronize(AudioData audioData);
	}

	public class SoloAudioSynchronizeLogic : SynchronizeLogic
	{
		public SoloAudioSynchronizeLogic(AudioSource audioSource) : base(audioSource){}
		
		protected override void OnStartSynchronize(AudioData audioData){}
	}
	
	public class RandomSynchronizeLogic : SynchronizeLogic
	{
		public RandomSynchronizeLogic(AudioSource audioSource) : base(audioSource){}
		
		protected override void OnStartSynchronize(AudioData audioData){}
	}

	public class PlaylistSynchronizeLogic : SynchronizeLogic
	{
		private readonly PlaylistPlayer _playlistPlayer;

		public PlaylistSynchronizeLogic(PlaylistPlayer playlistPlayer, AudioSource audioSource) : base(audioSource) =>
			_playlistPlayer = playlistPlayer;

		protected override void OnStartSynchronize(AudioData audioData) =>
			_playlistPlayer.Synchronize(audioData);
	}
}