using UnityEngine;

namespace RFG.Audio
{
	public class AudiTester : MonoBehaviour
	{
		private AudioPlayer _audioPlayer;

		[SerializeField] private string UK;
		[SerializeField] private string FI;
		[SerializeField] private string GO;
		[SerializeField] private string TP;

		private void Start()
		{
			_audioPlayer = new AudioPlayer();
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.U))
				_audioPlayer.Play(UK, transform.position);
			
			if(Input.GetKeyDown(KeyCode.F))
				_audioPlayer.Play(FI, transform.position);
			
			if(Input.GetKeyDown(KeyCode.G))
				_audioPlayer.Play(GO, transform.position);
			
			if(Input.GetKeyDown(KeyCode.T))
				_audioPlayer.Play(TP, transform.position);
			
			if(Input.GetKeyDown(KeyCode.K))
				_audioPlayer.Stop(UK);
			if(Input.GetKeyDown(KeyCode.I))
				_audioPlayer.Stop(FI);
			if(Input.GetKeyDown(KeyCode.O))
				_audioPlayer.Stop(GO);
			if(Input.GetKeyDown(KeyCode.P))
				_audioPlayer.Stop(TP);
		}
	}
}