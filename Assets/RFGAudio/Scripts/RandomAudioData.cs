using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
  [CreateAssetMenu(fileName = "New Random Audio Data", menuName = "RFG/Audio/Random Audio Data")]
  public class RandomAudioData : ScriptableObject
  {
    public List<AudioData> AudioList;
    public float WaitForSeconds = 3f;
    public float Offset = 5f;
  }
}