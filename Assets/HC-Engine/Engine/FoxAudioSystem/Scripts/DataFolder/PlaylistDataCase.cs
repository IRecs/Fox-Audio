using System.Collections.Generic;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.DataFolder
{
  [CreateAssetMenu(fileName = "New Playlist Data", menuName = "FoxAudioSystem/Audio/Data/Playlist/Playlist Case")]
  public class PlaylistDataCase :  AudioDataBase, ICase
  {
    public List<PlaylistAudioClipData> audioList;
    public bool loop = false;
    public float waitForSeconds = 1f;
    public float fadeTime = 1f;
    public int currentIndex = 0;
    public bool playOnAwake = false;

    public override AudioData DataObject => audioList[0];
    
    public void Initialize()
    {
      currentIndex = 0;
    }

    public AudioData GetCurrent()
    {
      return audioList[currentIndex];
    }

    public bool IsLast()
    {
      return currentIndex == audioList.Count - 1;
    }

    public bool IsFirst()
    {
      return currentIndex == 0;
    }

    public void Next()
    {
      int nextIndex = currentIndex + 1;
      if (nextIndex > audioList.Count - 1)
      {
        nextIndex = 0;
      }
      currentIndex = nextIndex;
    }

    public void Previous()
    {
      int nextIndex = currentIndex - 1;
      if (nextIndex < 0)
      {
        nextIndex = audioList.Count - 1;
      }
      currentIndex = nextIndex;
    }
    
    public void Add<T>(T newAsset) where T : ScriptableObject
    {
      if(newAsset is PlaylistAudioClipData playlistAudioClipData)
        audioList.Add(playlistAudioClipData);
    }
  }
}