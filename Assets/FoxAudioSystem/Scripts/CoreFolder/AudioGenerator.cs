using System;
using FoxAudioSystem.Scripts.ManagerFolder;
using FoxAudioSystem.Scripts.PlayersFolder;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class AudioGenerator
	{
		private readonly MainAudioCase _mainAudioCase;

		public AudioGenerator(MainAudioCase mainAudioCase)
		{
			_mainAudioCase = mainAudioCase;
			_mainAudioCase.Initialization();
		}

		public void Generate<T>(ref T newObject) where T : IAudioPlayer
		{
			GameObject audio = null;
			if(!_mainAudioCase.GetAudioPrefab(newObject.GetType(), ref audio))
				throw new Exception();

			newObject = Object.Instantiate(audio).GetComponent<T>();
		}
	}
}