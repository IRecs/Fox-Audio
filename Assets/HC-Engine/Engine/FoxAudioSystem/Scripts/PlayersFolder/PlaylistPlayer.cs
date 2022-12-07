using System.Collections;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ExtensionFolder;
using Unity.VisualScripting;
using UnityEngine;

namespace FoxAudioSystem.Scripts.PlayersFolder
{
  [AddComponentMenu("FoxAudioSystem/Audio/Playlist")]
  [RequireComponent(typeof(AudioSource))]
  public class PlaylistPlayer : AudioPlayerBase<PlaylistDataCase>
  {
    private bool _isPlaying;
    private Coroutine _playingCoroutine;
    public AudioData _currentData;
    private bool _waitePlayCurrentAudio;
    private PlaylistSynchronizeLogic _synchronizeLogic;

    public override ISynchronizeLogic SynchronizeLogic => _synchronizeLogic;

    protected override void OnInitialization(PlaylistDataCase dataCase)
    {
      _synchronizeLogic = new PlaylistSynchronizeLogic(this, AudioSource);
      Data.Initialize();
      
      if (Data.PlayOnAwake)
        Play();
    }

    public override AudioData GetData() =>
      _currentData;

    protected override void Reset()
    {
      _currentData = null;
      IsPause = false;
      if(_playingCoroutine != null)
        StopCoroutine(_playingCoroutine);
      
      _playingCoroutine = null;
    }

    public override void Play()
    {
      if (Data.AudioList.Count == 0)
        return;

      if(IsPause)
        UnPause();

      _playingCoroutine ??= StartCoroutine(PlayLogic());
    }
    
    private void UnPause()
    {
      IsPause = false;
      AudioSource.UnPause();
    }
    
    public override void Pause()
    {
      IsPause = true;
      AudioSource.Pause();
    }

    private IEnumerator PlayLogic()
    {
      _isPlaying = true;
      IsPause = false;

      yield return PlayCurrentAudio();

      while (_isPlaying)
      {
        if (Application.isFocused && !AudioSource.isPlaying && !IsPause)
        {
          if (Data.IsLast() && !Data.Loop)
          {
            _isPlaying = false;
            StopCoroutine(_playingCoroutine);
            continue;
          }

          if (_isPlaying)
          {
            Data.Next();
            yield return PlayCurrentAudio();
          }
        }
        yield return null;
      }

      _playingCoroutine = null;
    }

    private IEnumerator PlayCurrentAudio()
    {
      if(_waitePlayCurrentAudio)
        yield break;
      
      _waitePlayCurrentAudio = true;
      AudioData audioData = Data.GetCurrent();
      _currentData = Data.GetCurrent();
      audioData.GenerateAudioSource(gameObject);
      yield return new WaitForSecondsRealtime(Data.WaitForSeconds);
      yield return AudioSource.FadeIn(Data.FadeTime);
      _waitePlayCurrentAudio = false;
    }

    public void Synchronize(AudioData newAudioData)
    {
      if(!Data.SetToRef(newAudioData))
        return;
      Data.Previous();
      StopAudio();
      Play();
    }

    public override void Stop()
    {
      if(gameObject.activeInHierarchy)
        StartCoroutine(StopPlayLogic());
      else
        OnStop();
    }

    private IEnumerator StopPlayLogic()
    {
      StopAudio();
      yield return AudioSource.FadeOut(Data.FadeTime);
      OnStop();
      yield return null;
    }

    private void StopAudio()
    {
      if(_playingCoroutine != null)
        StopCoroutine(_playingCoroutine);
      
      _playingCoroutine = null;
      _isPlaying = false;
      AudioSource.Stop();
    }

    public void Next() =>
      StartCoroutine(NextAudio());

    private IEnumerator NextAudio()
    {
      yield return StopPlayLogic();
      Data.Next();
      Play();
    }

    public void Previous() =>
      StartCoroutine(PreviousCo());

    private IEnumerator PreviousCo()
    {
      yield return StopPlayLogic();
      Data.Previous();
      Play();
    }
  }
}