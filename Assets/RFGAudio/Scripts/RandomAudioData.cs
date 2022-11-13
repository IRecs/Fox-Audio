using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
  [CreateAssetMenu(fileName = "New Random Audio Data", menuName = "RFG/Audio/Random Audio Data")]
  public class RandomAudioData : AudioDataBase
  {
    public List<AudioData> audioList;
    public float waitForSeconds = 3f;
    public float minDistance = 30f;
    public float maxDistance = 35f;
    public override AudioData DataObject => audioList[0];
  }
}