using System;
using UnityEngine;

namespace RFG.Audio
{
  public interface IAudio
  {
    string Name { get; set; }
    Type Type {get; set; }
    public event Action<IAudio> End; 
    void GenerateAudioSource();
    void Play();
    void Stop();
    void Persist(bool persist);
    void SetPosition(Vector3 spawnPoint);
    void SetTarget(Transform target);
    GameObject GameObject { get; }
  }
}