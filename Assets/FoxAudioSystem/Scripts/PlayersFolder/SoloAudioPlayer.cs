using System.Collections;
using FoxAudioSystem.Scripts.CoreFolder;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ExtensionFolder;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
	[AddComponentMenu("FoxAudioSystem/Audio/Audio")]
	public class SoloAudioPlayer : AudioPlayerBase<SoloAudioClipData>
	{
		private int _playTime;
		private float _timeLeft;

		private Coroutine _waitEndAudioCoroutine;
		private SoloAudioSynchronizeLogic _synchronizeLogic;

		public override ISynchronizeLogic SynchronizeLogic => _synchronizeLogic;
		
		protected override void OnInitialization(SoloAudioClipData data) =>
			_synchronizeLogic = new SoloAudioSynchronizeLogic(AudioSource);

		public override void Play()
		{
			if(IsPause)
				Continue();
			else
				PlayNew();

			if(!Data.loop)
				_waitEndAudioCoroutine = StartCoroutine(WaitEndAudio());
		}

		private void PlayNew()
		{
			_timeLeft = Data.clip.length;

			SetPitch();
			TrySynchronize();
			StartPlay();
		}

		private void Continue()
		{
			Synchronize(_playTime);
			AudioSource.UnPause();
			IsPause = false;
		}

		public override void Pause()
		{
			StopWaitEndAudio();

			IsPause = true;
			_timeLeft = Data.clip.length - AudioSource.time;
			_playTime = AudioSource.timeSamples;
			AudioSource.Pause();
		}

		public override void Stop()
		{
			if(Data.fadeTime != 0f)
				StartCoroutine(AudioSource.FadeOut(Data.fadeTime));
			else
			{
				AudioSource.Stop();
				OnStop();
			}
		}

		protected override void Reset()
		{
			IsPause = false;
			_waitEndAudioCoroutine = null;
			_playTime = -1;
		}

		private void SetPitch()
		{
			if(Data.randomPitch)
				AudioSource.pitch = Random.Range(Data.minPitch, Data.maxPitch);
		}

		private void TrySynchronize()
		{
			if(Data.Synchronize)
				StartSynchronize();
		}

		private void StartSynchronize() =>
			AudioSynchronizerCase.Add(this);

		public void Synchronize(int timeSamples) =>
			AudioSource.timeSamples = timeSamples;

		public void SynchronizeVolume(float volumeMultiplier) =>
			AudioSource.volume = Data.volume * volumeMultiplier;

		private IEnumerator WaitEndAudio()
		{
			yield return new WaitForSeconds(_timeLeft);
			_waitEndAudioCoroutine = null;
			Stop();
		}

		private void StartPlay() =>
			StartCoroutine(AudioSource.FadeIn(Data.fadeTime));

		private void StopWaitEndAudio()
		{
			StopCoroutine(_waitEndAudioCoroutine);
			_waitEndAudioCoroutine = null;
		}
	}
}