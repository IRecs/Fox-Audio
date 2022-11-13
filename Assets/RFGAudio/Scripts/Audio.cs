using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Audio")]
  public class Audio : AudioBase<AudioData>
  {
    public override void Play()
    {
      if (Data.randomPitch)
        AudioSource.pitch = Random.Range(Data.minPitch, Data.maxPitch);
      
      if(Data.fadeTime != 0f)
        StartCoroutine(AudioSource.FadeIn(Data.fadeTime));
      else
        AudioSource.Play();
    }

    public override void Stop()
    {
      if(Data.fadeTime != 0f)
        StartCoroutine(AudioSource.FadeOut(Data.fadeTime));
      else
        AudioSource.Stop();
    }

    public void Pause() =>
      AudioSource.Pause();
  }
}