using FoxAudioSystem.Editor;
using FoxAudioSystem.Scripts.CoreFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.TestFolder
{
	public class AudioTest : MonoBehaviour
	{
		private AudioPlayer _audioPlayer;


		private void Start()
		{
			_audioPlayer = new AudioPlayer();
		}

		private void Update()
		{
			/*if(Input.GetKeyDown(KeyCode.U))
				_audioPlayer.Play(AudioKey.Monetki, transform.position);
			
			if(Input.GetKeyDown(KeyCode.T))
				_audioPlayer.Stop(AudioKey.Monetki);*/
		}
	}
}