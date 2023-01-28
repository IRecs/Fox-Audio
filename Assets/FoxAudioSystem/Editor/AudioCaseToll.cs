using System;
using FoxAudioSystem.Scripts.ManagerFolder;

namespace FoxAudioSystem.EditorFolder
{
	public struct AudioCaseToll
	{
		public readonly string Name;
		public readonly Action<string, SubCaseAudioGroup> Colbeck;

		public AudioCaseToll(Action<string, SubCaseAudioGroup> colbeck, string name)
		{
			Colbeck = colbeck;
			Name = name;
		}
	}
}