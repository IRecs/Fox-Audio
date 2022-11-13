using UnityEngine;

namespace RFG.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class AudioBase<T> : MonoBehaviour, IAudio where T : AudioDataBase
	{
		[field:SerializeField] public T Data { get; private set; }
		protected AudioSource AudioSource;

		private void Awake() =>
			AudioSource = GetComponent<AudioSource>();

		public void Initialization(T playlistData)
		{
			Data = playlistData;
			AudioSource ??= GetComponent<AudioSource>();
			OnInitialization();
		}

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