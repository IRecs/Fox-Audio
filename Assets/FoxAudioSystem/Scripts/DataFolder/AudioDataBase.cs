using UnityEngine;

namespace FoxAudioSystem.Scripts.DataFolder
{
	public abstract class AudioDataBase : ScriptableObject
	{
    public abstract AudioData DataObject { get; }
	}
}