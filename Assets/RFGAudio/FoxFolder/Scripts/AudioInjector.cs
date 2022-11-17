/*using System;
using Engine.DI;
using UnityEngine;

namespace RFG.Audio
{

    public class AudioInjector : MonoBehaviour, IDependency
    {
        [SerializeField] private FoxAudioManager _prefab;
		
        public void Inject()
        {
            if(FoxAudioManager.Instance == null)
                Instantiate(_prefab).Initialization();
			
            DIContainer.RegisterAsSingle<IFoxAudioManager>(FoxAudioManager.Instance);
        }

        private void Start()
        {
            FoxAudioManager.Instance.OnStart();
        }
    }
   
} */