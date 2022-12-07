using System;
using FoxAudioSystem.Scripts;
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
			helpString = Constants.CaseSelectorBuilder.HelpString;

		private void CheckError()
		{
			string error = "";

			if(Case == null)
				error +=  Constants.CaseSelectorBuilder.CaseIsNull;

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
			DisplayWizard<SubCaseSelectorBuilder>(Constants.CaseSelectorBuilder.ButtonName);
	}
	
	public class RandomAudioDataCaseSelectorBuilder : CaseSelectorBuilder<RandomAudioDataCase>
	{
		public static RandomAudioDataCaseSelectorBuilder CreateWizard() =>
			DisplayWizard<RandomAudioDataCaseSelectorBuilder>(Constants.CaseSelectorBuilder.ButtonName);
	}
	
	public class PlaylistDataCaseSelectorBuilder : CaseSelectorBuilder<PlaylistDataCase>
	{
		public static PlaylistDataCaseSelectorBuilder  CreateWizard() =>
			DisplayWizard<PlaylistDataCaseSelectorBuilder >(Constants.CaseSelectorBuilder.ButtonName);
	}
}