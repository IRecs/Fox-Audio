using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Audio")]
  public class Audio : MonoBehaviour
  {
    [field: SerializeField] public AudioData AudioData { get; set; }
    private AudioSource _audioSource;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
      if (AudioData.RandomPitch)
      {
        _audioSource.pitch = UnityEngine.Random.Range(AudioData.MinPitch, AudioData.MaxPitch);
      }
      if (AudioData.FadeTime != 0f)
      {
        StartCoroutine(_audioSource.FadeIn(AudioData.FadeTime));
      }
      else
      {
        _audioSource.Play();
      }
    }

    public void Stop()
    {
      if (AudioData.FadeTime != 0f)
      {
        StartCoroutine(_audioSource.FadeOut(AudioData.FadeTime));
      }
      else
      {
        _audioSource.Stop();
      }
    }

    public void Pause()
    {
      _audioSource.Pause();
    }
  }
}