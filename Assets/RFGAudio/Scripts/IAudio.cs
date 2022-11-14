using System;
using UnityEngine;

namespace RFG.Audio
{
  public interface IAudio
  {
    public event Action<GameObject> End; 
    void GenerateAudioSource();
    void Play();
    void Stop();
    void Persist(bool persist);
    void SetPosition(Vector3 spawnPoint);
    void SetTarget(Transform target);
  }
}