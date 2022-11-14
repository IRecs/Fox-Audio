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
		
		public event Action<GameObject> End;

		private void Awake() =>
			AudioSource = GetComponent<AudioSource>();

		private void LateUpdate()
		{
			if(_target == null)
				return;
			
			transform.position = _target.position;
		}

		public void Initialization(T playlistData, bool persist)
		{
			Data = playlistData;
			GenerateAudioSource();
			Persist(persist);
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

		public void Persist(bool persist)
		{
			if(persist)
				DontDestroyOnLoad(gameObject);
			else
				UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
		}

		public void GenerateAudioSource() =>
			Data.DataObject.GenerateAudioSource(gameObject);
	}
}