using System;
using FoxAudioSystem.Scripts.DataFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
	public interface IAudioPlayer
	{
		string Name { get; }
		string ID { get; set; }
		Type Type { get; set; }
		public event Action<IAudioPlayer> End;
		void GenerateAudioSource();
		void Play();
		void Pause();
		void Stop();
		void SetPosition(Vector3 spawnPoint);
		void SetTarget(Transform target);
		GameObject GameObject { get; }
		ISynchronizeLogic SynchronizeLogic { get; }
		AudioData GetData();
	}
}