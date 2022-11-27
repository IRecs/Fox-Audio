using System.Collections.Generic;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.DataFolder
{
  [CreateAssetMenu(fileName = "New Random Audio Data", menuName = "FoxAudioSystem/Audio/Data/Random/Random Case")]
  public class RandomAudioDataCase : AudioDataBase, ICase
  {
    public List<RandomAudioClipData> audioList;
    public float waitForSeconds = 3f;
    public float minDistance = 30f;
    public float maxDistance = 35f;
    public bool isLoop;
    public override AudioData DataObject => audioList[0];
    
    public void Add<T>(T newAsset) where T : ScriptableObject
    {
      if(newAsset is RandomAudioClipData randomAudioClipData)
        audioList.Add(randomAudioClipData);
    }
  }
}