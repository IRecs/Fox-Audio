using System;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEditor;
using UnityEngine;

#pragma warning disable 
namespace FoxAudioSystem.EditorFolder
{
	public class CaseSelectorBuilder<T> : ScriptableWizard where T :ScriptableObject, ICase 
	{
		public T Case;
		public event Action<T> Close;
		
		public void OnWizardUpdate()
		{
			ShowHelpString();
			CheckError();
		}

		private void ShowHelpString() =>
			helpString = "Choose which group you want to add\n";

		private void CheckError()
		{
			string error = "";

			if(Case == null)
				error += "Case is null!\n";

			errorString = error;
			isValid = errorString.Length == 0;
		}

		public void OnWizardCreate()
		{
			Close?.Invoke(Case);
		}
	}

	public class SubCaseSelectorBuilder : CaseSelectorBuilder<SubCaseAudioGroup>
	{
		public static SubCaseSelectorBuilder CreateWizard() =>
			DisplayWizard<SubCaseSelectorBuilder>("Create Audio");
	}
	
	public class RandomAudioDataCaseSelectorBuilder : CaseSelectorBuilder<RandomAudioDataCase>
	{
		public static RandomAudioDataCaseSelectorBuilder CreateWizard() =>
			DisplayWizard<RandomAudioDataCaseSelectorBuilder>("Create Audio");
	}
	
	public class PlaylistDataCaseSelectorBuilder : CaseSelectorBuilder<PlaylistDataCase>
	{
		public static PlaylistDataCaseSelectorBuilder  CreateWizard() =>
			DisplayWizard<PlaylistDataCaseSelectorBuilder >("Create Audio");
	}
}