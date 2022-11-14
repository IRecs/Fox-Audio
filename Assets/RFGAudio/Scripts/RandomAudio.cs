using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Random Audio")]
  public class RandomAudio : AudioBase<RandomAudioData>
  {
    private float _waitDuration = 0f;
    private int _lastIndex;
    private bool _isPlaying = true;
    private AudioData _currentAudioData;
  
    private void LateUpdate()
    {
      if (!_isPlaying)
        return;
      
      _waitDuration += Time.deltaTime;
      
      if (_waitDuration > Data.waitForSeconds)
        PlayRandom();
    }

    public void PlayRandom()
    {
      _waitDuration = 0f;
      int randomIndex = UnityEngine.Random.Range(0, Data.audioList.Count - 1);
      
      if (randomIndex == _lastIndex)
      {
        PlayRandom();
        return;
      }
      
      _lastIndex = randomIndex;
      _currentAudioData = Data.audioList[randomIndex];
      _currentAudioData.GenerateAudioSource(gameObject);
      transform.localPosition = GetRandomPosition();

      if(_currentAudioData.fadeTime != 0)
        StartCoroutine(AudioSource.FadeIn(_currentAudioData.fadeTime));
      else
        AudioSource.Play();
    }

    private Vector3 GetRandomPosition()
    {
      Vector3 offset = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0).normalized * Random.Range(Data.minDistance, Data.maxDistance);
      return Vector3.zero + offset;
    }

    public override void Play() =>
      _isPlaying = true;

    public override void Stop()
    {
      _isPlaying = false;
      
      if(_currentAudioData.fadeTime != 0)
        StartCoroutine(AudioSource.FadeOut(_currentAudioData.fadeTime));
      else
        AudioSource.Stop();
    }
  }
}