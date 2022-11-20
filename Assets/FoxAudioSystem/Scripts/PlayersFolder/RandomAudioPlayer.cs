using System;
using System.Collections;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ExtensionFolder;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
	[AddComponentMenu("FoxAudioSystem/Audio/Random Audio")]
	public class RandomAudioPlayer : AudioBase<RandomAudioData>
	{
		private int _lastIndex;
		private bool _isPlaying = true;
		private AudioData _currentAudioData;
		private int _countPlay = 0;

		public void PlayRandom()
		{
			_countPlay++;
			int randomIndex = Random.Range(0, Data.audioList.Count - 1);

			if(randomIndex == _lastIndex)
			{
				if(Data.audioList.Count <= 2)
				{
					randomIndex = Math.Max(Data.audioList.Count - 1, randomIndex == 0 ? 1 : 0);
				}
				else
				{
					PlayRandom();
					return;
				}
			}

			_lastIndex = randomIndex;
			_currentAudioData = Data.audioList[randomIndex];
			_currentAudioData.GenerateAudioSource(gameObject);

			if(_currentAudioData.fadeTime != 0)
				StartCoroutine(AudioSource.FadeIn(_currentAudioData.fadeTime));
			else
			{
				AudioSource.Play();
				StartCoroutine(WaitePlaying());
			}
		}

		private IEnumerator WaitePlaying()
		{
			yield return new WaitForSecondsRealtime(_currentAudioData.clip.length);

			if(!_isPlaying)
				yield break;

			if(Data.isLoop || _countPlay < 1)
				PlayRandom();
			else
				Stop();
		}

		public override void Play()
		{
			_countPlay = 0;
			PlayRandom();
		}

		public override void Stop()
		{
			_isPlaying = false;

			if(gameObject == null && !gameObject.activeInHierarchy)
				return;

			if(_currentAudioData.fadeTime != 0)
			{
				StartCoroutine(AudioSource.FadeOut(_currentAudioData.fadeTime));
			}
			else
			{
				AudioSource.Stop();
				OnStop();
			}

		}
	}
}