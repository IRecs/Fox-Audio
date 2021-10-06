using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Random Audio")]
  public class RandomAudio : MonoBehaviour, IAudio
  {
    public RandomAudioData RandomAudioData;

    private AudioSource _audioSource;
    private float _waitDuration = 0f;
    private Camera _mainCamera;
    private int _lastIndex;
    private bool _isPlaying = true;
    private AudioData _currentAudioData;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();
      _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
      if (!_isPlaying)
      {
        return;
      }
      _waitDuration += Time.deltaTime;
      if (_waitDuration > RandomAudioData.waitForSeconds)
      {
        PlayRandom();
      }
    }

    public void PlayRandom()
    {
      _waitDuration = 0f;
      int randomIndex = UnityEngine.Random.Range(0, RandomAudioData.audioList.Count - 1);
      if (randomIndex == _lastIndex)
      {
        PlayRandom();
        return;
      }
      _lastIndex = randomIndex;
      _currentAudioData = RandomAudioData.audioList[randomIndex];
      _currentAudioData.GenerateAudioSource(gameObject);
      transform.position = GetRandomScreenPoint();
      if (_currentAudioData.fadeTime != 0)
      {
        StartCoroutine(_audioSource.FadeIn(_currentAudioData.fadeTime));
      }
      else
      {
        _audioSource.Play();
      }
    }

    private Vector3 GetRandomScreenPoint()
    {
      Vector3 point = new Vector3(
        UnityEngine.Random.Range(0, Screen.width),
        UnityEngine.Random.Range(0, Screen.height),
        0
      );
      return _mainCamera.ScreenToWorldPoint(point * RandomAudioData.offset);
    }

    public void Play()
    {
      _isPlaying = true;
    }

    public void Stop()
    {
      _isPlaying = false;
      if (_currentAudioData.fadeTime != 0)
      {
        StartCoroutine(_audioSource.FadeOut(_currentAudioData.fadeTime));
      }
      else
      {
        _audioSource.Stop();
      }
    }

    public void GenerateAudioSource()
    {
      RandomAudioData.audioList[0].GenerateAudioSource(gameObject);
    }
  }
}