using System.Collections.Generic;
using UnityEngine;

namespace FoxAudioSystem.Scripts.DataFolder
{
  [CreateAssetMenu(fileName = "New Random Audio Data", menuName = "FoxAudioSystem/Audio/Data/Random/Random Case")]
  public class RandomAudioDataCase : AudioDataBase
  {
    public List<RandomAudioClipData> audioList;
    public float waitForSeconds = 3f;
    public float minDistance = 30f;
    public float maxDistance = 35f;
    public bool isLoop;
    public override AudioData DataObject => audioList[0];
  }
}