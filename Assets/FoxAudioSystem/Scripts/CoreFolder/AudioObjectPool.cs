using System;
using System.Collections.Generic;
using System.Linq;
using FoxAudioSystem.Scripts.PlayersFolder;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class AudioObjectPool
	{
		private Dictionary<Type, List<IAudioPlayer>> _objects;

		public AudioObjectPool()
		{
			_objects = new Dictionary<Type, List<IAudioPlayer>>();
			_objects.Add(typeof(SoloAudioPlayer), new List<IAudioPlayer>());
			_objects.Add(typeof(PlaylistPlayer), new List<IAudioPlayer>());
			_objects.Add(typeof(RandomAudioPlayer), new List<IAudioPlayer>());
		}

		public bool Get<T>(ref T getobject) where T : IAudioPlayer
		{
			if(_objects[getobject.GetType()].Count <= 0)
				return false;

			IAudioPlayer audioPlayer = _objects[getobject.GetType()].First();
			_objects[getobject.GetType()].Remove(audioPlayer);
			
			getobject = (T) audioPlayer;
			return true;
		}

		public void Add(Type type, IAudioPlayer audioPlayer)
		{
			if(!_objects.ContainsKey(type))
				throw new Exception("Is not IAudio");
			
			_objects[type].Add(audioPlayer);
		}
	}
}