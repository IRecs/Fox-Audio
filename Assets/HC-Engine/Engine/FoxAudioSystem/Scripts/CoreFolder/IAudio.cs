using System;
using UnityEngine;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public interface IAudio
	{
		string Name { get; }
		string ID { get; set; }
		Type Type { get; set; }
		public event Action<IAudio> End;
		void GenerateAudioSource();
		void Play();
		void Stop();
		void SetPosition(Vector3 spawnPoint);
		void SetTarget(Transform target);
		GameObject GameObject { get; }

	}

	public interface ISynchronizedSound
	{
		int TimeSamples { get; }
		void Synchronize(int timeSamples);
		void SynchronizeVolume(float volumeMultiplier);
	}

}