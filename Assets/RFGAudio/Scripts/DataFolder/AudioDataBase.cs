using UnityEngine;

namespace RFG.Audio
{
	public abstract class AudioDataBase : ScriptableObject
	{
    public abstract AudioData DataObject { get; }
	}
}