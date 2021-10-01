using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
  [CreateAssetMenu(fileName = "New Playlist Data", menuName = "RFG/Audio/Playlist Data")]
  public class PlaylistData : ScriptableObject
  {
    public List<AudioData> AudioList;
    public bool Loop = false;
    public float WaitForSeconds = 1f;
    public float FadeTime = 1f;
    public int CurrentIndex = 0;
    public bool PlayOnStart = false;

    public void Initialize()
    {
      CurrentIndex = 0;
    }

    public AudioData GetCurrent()
    {
      return AudioList[CurrentIndex];
    }

    public bool IsLast()
    {
      return CurrentIndex == AudioList.Count - 1;
    }

    public bool IsFirst()
    {
      return CurrentIndex == 0;
    }

    public void Next()
    {
      int nextIndex = CurrentIndex + 1;
      if (nextIndex > AudioList.Count - 1)
      {
        nextIndex = 0;
      }
      CurrentIndex = nextIndex;
    }

    public void Previous()
    {
      int nextIndex = CurrentIndex - 1;
      if (nextIndex < 0)
      {
        nextIndex = AudioList.Count - 1;
      }
      CurrentIndex = nextIndex;
    }
  }
}