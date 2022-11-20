using System.Collections;
using FoxAudioSystem.Scripts.CoreFolder;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ExtensionFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
  [AddComponentMenu("FoxAudioSystem/Audio/Audio")]
  public class SoloAudioPlayer : AudioBase<SoloAudioClipData> , ISynchronizedSound
  {
    protected override void OnInitialization(SoloAudioClipData data)
    {}
    
    public override void Play()
    {
      if (Data.randomPitch)
        AudioSource.pitch = Random.Range(Data.minPitch, Data.maxPitch);

      if(Data.fadeTime != 0f)
      {
        StartCoroutine(AudioSource.FadeIn(Data.fadeTime));
      }
      else
      {
        if(Data.Synchronize)
          StartSynchronize();
        
        AudioSource.Play();
        if(!Data.loop)
          StartCoroutine(WaitEndAudio());
      }
    }
    
    public void Synchronize(int timeSamples) =>
      AudioSource.timeSamples = timeSamples;

    public void SynchronizeVolume(float volumeMultiplier) =>
      AudioSource.volume = Data.volume * volumeMultiplier;
    
    private void StartSynchronize() =>
      AudioSynchronizerCase.Add(this);

    private IEnumerator WaitEndAudio()
    {
      yield return new WaitForSecondsRealtime(Data.clip.length);
      Stop();
    }

    public override void Stop()
    {
      if(Data.fadeTime != 0f)
        StartCoroutine(AudioSource.FadeOut(Data.fadeTime));
      else
      {
        AudioSource.Stop();
        OnStop();
      }
    }

    public void Pause() =>
      AudioSource.Pause();
  }
}