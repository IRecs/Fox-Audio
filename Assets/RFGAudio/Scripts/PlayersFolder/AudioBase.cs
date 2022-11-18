using System;
using UnityEngine;

namespace RFG.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class AudioBase<T> : MonoBehaviour, IAudio where T : AudioDataBase
	{
		[field:SerializeField] public T Data { get; private set; }
		protected AudioSource AudioSource;
		private Transform _target;

		public string ID { get; set; }
		public string Name { get; set; }
		public Type Type { get; set; }
		
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

		public void Initialization(T playlistData)
		{
			Data = playlistData;
			GenerateAudioSource();
			AudioSource ??= GetComponent<AudioSource>();
			OnInitialization();
		}
		
		public void SetPosition(Vector3 spawnPoint) =>
			transform.position = spawnPoint;

		public void SetTarget(Transform target) =>
			_target = target;

		protected virtual void OnInitialization(){}

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