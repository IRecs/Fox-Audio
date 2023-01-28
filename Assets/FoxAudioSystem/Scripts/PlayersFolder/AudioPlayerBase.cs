using System;
using FoxAudioSystem.Scripts.CoreFolder;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ExtensionFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class AudioPlayerBase<T> : MonoBehaviour, IAudioPlayer where T : AudioDataBase
	{
		[field: SerializeField] public T Data { get; private set; }
		
		protected AudioSource AudioSource;
		protected bool IsPause;
		
		private Transform _target;

		public string ID { get; set; }
		public string Name { get; private set; }
		public Type Type { get; set; }
		public abstract ISynchronizeLogic SynchronizeLogic { get; }

		public int TimeSamples => AudioSource.timeSamples;
		public event Action<IAudioPlayer> End;

		public GameObject GameObject => gameObject;
		public bool CanPause => !IsPause;

		private void Awake() =>
			AudioSource = GetComponent<AudioSource>();

		public abstract void Play();
		public abstract void Pause();
		public abstract void Stop();

		private void LateUpdate() =>
			PursuitGoal();

		public void Initialization(T data, string name)
		{
			Name = name;
			Data = data;
			GenerateAudioSource();
			AudioSource ??= GetComponent<AudioSource>();
			OnInitialization(data);
		}

		public void SetPosition(Vector3 spawnPoint) =>
			transform.position = spawnPoint;

		public void SetTarget(Transform target) =>
			_target = target;

		private void PursuitGoal()
		{
			if(_target != null)
				transform.position = _target.position;
		}

		protected virtual void OnInitialization(T data) { }

		protected void OnStop()
		{
			_target = null;
			End?.Invoke(this);
			Reset();
		}

		protected abstract void Reset();

		public void GenerateAudioSource() =>
			Data.DataObject.GenerateAudioSource(gameObject);

		private void OnDisable() =>
			Stop();


		public virtual AudioData GetData() =>
			Data.DataObject;
	}
}