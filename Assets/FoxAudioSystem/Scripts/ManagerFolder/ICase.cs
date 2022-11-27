using FoxAudioSystem.Scripts.DataFolder;
using UnityEngine;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
	public interface ICase
	{
		void Add<T>(T newAsset) where T : ScriptableObject;
	}
}