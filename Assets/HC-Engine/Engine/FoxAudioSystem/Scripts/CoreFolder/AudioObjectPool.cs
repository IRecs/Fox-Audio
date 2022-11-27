﻿using System;
using System.Collections.Generic;
using System.Linq;
using FoxAudioSystem.Scripts.PlayersFolder;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class AudioObjectPool
	{
		private Dictionary<Type, List<IAudio>> _objects;

		public AudioObjectPool()
		{
			_objects = new Dictionary<Type, List<IAudio>>();
			_objects.Add(typeof(SoloAudioPlayer), new List<IAudio>());
			_objects.Add(typeof(PlaylistPLayer), new List<IAudio>());
			_objects.Add(typeof(RandomAudioPlayer), new List<IAudio>());
		}

		public bool Get<T>(ref T getobject) where T : IAudio
		{
			if(_objects[getobject.GetType()].Count <= 0)
				return false;

			IAudio audio = _objects[getobject.GetType()].First();
			_objects[getobject.GetType()].Remove(audio);
			
			getobject = (T) audio;
			return true;
		}

		public void Add(Type type, IAudio audio)
		{
			if(!_objects.ContainsKey(type))
				throw new Exception("Is not IAudio");
			
			_objects[type].Add(audio);
		}
	}
}