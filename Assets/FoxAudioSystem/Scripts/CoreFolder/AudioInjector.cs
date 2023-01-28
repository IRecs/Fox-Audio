using Engine.DI;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.CoreFolder
{

	public class AudioInjector : MonoBehaviour, IDependency
	{
		[SerializeField] private FoxAudioManager _prefab;

		private void Awake() =>
			Inject();

		public void Inject()
		{
			if(FoxAudioManager.Instance == null)
				Instantiate(_prefab).Initialization();

			DIContainer.RegisterAsSingle<IFoxAudioManager>(FoxAudioManager.Instance);
		}

		private void Start() =>
			FoxAudioManager.Instance.OnStart();
	}

}