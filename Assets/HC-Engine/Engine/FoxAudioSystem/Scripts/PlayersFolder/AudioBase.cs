using System;
using FoxAudioSystem.Scripts.CoreFolder;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ExtensionFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class AudioBase<T> : MonoBehaviour, IAudio where T : AudioDataBase
	{
		[field: SerializeField] public T Data { get; private set; }
		protected AudioSource AudioSource;
		private Transform _target;

		public string ID { get; set; }
		public string Name { get; private set; }
		public Type Type { get; set; }
		
		public int TimeSamples => AudioSource.timeSamples;

		public event Action<IAudio> End;

		public GameObject GameObject => gameObject;

		private void Awake() =>
			AudioSource = GetComponent<AudioSource>();

		private void LateUpdate()
		{
			if(_target == null)
				return;

			transform.position = _target.position;
		}

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

		protected virtual void OnInitialization(T data) { }

		public abstract void Play();

		public abstract void Stop();

		protected void OnStop()
		{
			_target = null;
			End?.Invoke(this);
		}

		public void GenerateAudioSource() =>
			Data.DataObject.GenerateAudioSource(gameObject);

		private void OnDisable() =>
			Stop();
	}
}