using System.Collections;
using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Playlist")]
  [RequireComponent(typeof(AudioSource))]
  public class Playlist : AudioBase<PlaylistData>
  {
    private bool _isPlaying;
    private bool _isPaused;
    private IEnumerator _playingCoroutine;
    private bool _witePlayCurrentAudio;

    protected override void OnInitialization()
    {
      Data.Initialize();
      
      if (Data.playOnAwake)
        Play();
    }

    public override void Play()
    {
      if (Data.audioList.Count == 0)
        return;

      _playingCoroutine = PlayCo();
      StartCoroutine(_playingCoroutine);
    }

    private IEnumerator PlayCo()
    {
      _isPlaying = true;
      _isPaused = false;

      yield return PlayCurrentAudio();

      while (_isPlaying)
      {
        if (Application.isFocused && !AudioSource.isPlaying && !_isPaused)
        {
          if (Data.IsLast() && !Data.loop)
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
    }

    private IEnumerator PlayCurrentAudio()
    {
      if(_witePlayCurrentAudio)
        yield break;
      
      _witePlayCurrentAudio = true;
      AudioData audioData = Data.GetCurrent();
      audioData.GenerateAudioSource(gameObject);
      yield return new WaitForSecondsRealtime(Data.waitForSeconds);
      yield return AudioSource.FadeIn(Data.fadeTime);
      _witePlayCurrentAudio = false;
    }

    public void TogglePause()
    {
      if (_isPaused)
      {
        _isPaused = false;
        AudioSource.UnPause();
      }
      else
      {
        _isPaused = true;
        AudioSource.Pause();
      }
    }

    public override void Stop() =>
      StartCoroutine(StopCo());

    private IEnumerator StopCo()
    {
      StopCoroutine(_playingCoroutine);
      _isPlaying = false;
      yield return AudioSource.FadeOut(Data.fadeTime);
      yield return null;
    }

    public void Next() =>
      StartCoroutine(NextCo());

    private IEnumerator NextCo()
    {
      yield return StopCo();
      Data.Next();
      Play();
    }

    public void Previous() =>
      StartCoroutine(PreviousCo());

    private IEnumerator PreviousCo()
    {
      yield return StopCo();
      Data.Previous();
      Play();
    }
  }
}