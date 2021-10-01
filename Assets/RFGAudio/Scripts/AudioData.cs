using UnityEngine;
using UnityEngine.Audio;

namespace RFG.Audio
{
  [CreateAssetMenu(fileName = "New Audio Data", menuName = "RFG/Audio/Audio Data")]
  public class AudioData : ScriptableObject
  {
    public AudioClip Clip;
    public AudioMixerGroup Output;

    [Range(0, 1)]
    public float Volume = 1f;

    [Range(.25f, 3f)]
    public float Pitch = 1f;

    public bool RandomPitch = false;

    [Range(0f, 1f)]
    public float MinPitch;
    [Range(0f, 1f)]
    public float MaxPitch;


    public bool PlayOnAwake = false;
    public bool Loop = false;
    public float FadeTime = 0f;

    [Range(0f, 1f)]
    public float SpacialBlend = 1f;
    public float MinDistance = 1f;
    public float MaxDistance = 100f;
    public AudioRolloffMode RolloffMode;

  }
}