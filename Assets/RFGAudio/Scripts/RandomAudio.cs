using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Random Audio")]
  public class RandomAudio : MonoBehaviour
  {
    public RandomAudioData RandomAudioData;

    private AudioSource _audioSource;
    private float _waitDuration = 0f;
    private Camera _mainCamera;
    private int _lastIndex;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();
      _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
      _waitDuration += Time.deltaTime;
      if (_waitDuration > RandomAudioData.WaitForSeconds)
      {
        PlayRandom();
      }
    }

    public void PlayRandom()
    {
      _waitDuration = 0f;
      int randomIndex = UnityEngine.Random.Range(0, RandomAudioData.AudioList.Count - 1);
      if (randomIndex == _lastIndex)
      {
        PlayRandom();
        return;
      }
      _lastIndex = randomIndex;
      AudioData audioData = RandomAudioData.AudioList[randomIndex];
      audioData.GenerateAudioSource(gameObject);
      transform.position = GetRandomScreenPoint();
      if (audioData.FadeTime != 0)
      {
        StartCoroutine(_audioSource.FadeIn(audioData.FadeTime));
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
      return _mainCamera.ScreenToWorldPoint(point * RandomAudioData.Offset);
    }
  }
}