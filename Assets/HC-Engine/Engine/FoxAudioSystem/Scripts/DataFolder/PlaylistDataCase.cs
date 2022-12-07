using System.Collections.Generic;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.DataFolder
{
	[CreateAssetMenu(fileName = "New Playlist Data", menuName = "FoxAudioSystem/Audio/Data/Playlist/Playlist Case")]
	public class PlaylistDataCase : AudioDataBase, ICase
	{
		[field: SerializeField] public List<PlaylistAudioClipData> AudioList{ get; private set; }
		[field: SerializeField] public bool Loop { get; private set; } = false;
		[field: SerializeField] public float WaitForSeconds { get; private set; } = 1;
		[field: SerializeField] public float FadeTime { get; private set; } = 1;
		[field: SerializeField] public bool PlayOnAwake  { get; private set; } = false;

		private int _currentIndex = 0;

		public override AudioData DataObject =>
			AudioList[0];

		public void SetAudioList(List<PlaylistAudioClipData> audioList) =>
			AudioList = audioList;
		
		public void Initialize() =>
			_currentIndex = 0;

		public AudioData GetCurrent() =>
			AudioList[_currentIndex];

		public bool IsLast() =>
			_currentIndex == AudioList.Count - 1;

		public bool IsFirst() =>
			_currentIndex == 0;

		public void Next()
		{
			int nextIndex = _currentIndex + 1;
			if(nextIndex > AudioList.Count - 1)
			{
				nextIndex = 0;
			}
			_currentIndex = nextIndex;
		}

		public void Previous()
		{
			int nextIndex = _currentIndex - 1;
			if(nextIndex < 0)
			{
				nextIndex = AudioList.Count - 1;
			}
			_currentIndex = nextIndex;
		}

		public void Add<T>(T newAsset) where T : ScriptableObject
		{
			if(newAsset is PlaylistAudioClipData playlistAudioClipData)
				AudioList.Add(playlistAudioClipData);
		}

		public bool SetToRef(AudioData data)
		{
			if(!(data is PlaylistAudioClipData listData))
				return false;

			if(!AudioList.Contains(listData))
				return false;
			
			int newIndex = AudioList.IndexOf(listData);

			if(newIndex == _currentIndex)
				return false;
			
			_currentIndex = newIndex ;
			return true;
		}
	}
}