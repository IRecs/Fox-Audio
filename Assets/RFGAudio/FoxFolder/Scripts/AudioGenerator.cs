﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RFG.Audio
{
	public class AudioGenerator
	{
		private readonly MainAudioCase _mainAudioCase;

		public AudioGenerator(MainAudioCase mainAudioCase)
		{
			_mainAudioCase = mainAudioCase;
			_mainAudioCase.Initialization();
		}

		public void Generate<T>(ref T newObject) where T : IAudio
		{
			GameObject audio = null;
			if(!_mainAudioCase.GetAudioPrefab(newObject.GetType(), ref audio))
				throw new Exception();

			newObject = Object.Instantiate(audio).GetComponent<T>();
		}
	}
}