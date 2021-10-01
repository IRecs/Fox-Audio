using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Audio Manager")]
  public class AudioManager : MonoBehaviour
  {
    public List<AudioMixerSettings> AudioMixerSettings;

    private void Start()
    {
      foreach (AudioMixerSettings settings in AudioMixerSettings)
      {
        settings.Initialize();
      }
    }
  }
}