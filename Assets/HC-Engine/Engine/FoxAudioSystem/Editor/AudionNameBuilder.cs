using System.Collections.Generic;
using System.IO;
using System.Linq;
using FoxAudioSystem.Scripts;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEditor;

#pragma warning disable
namespace FoxAudioSystem.EditorFolder
{
	public class AudionNameBuilder : SubCaseSelectorBuilder
	{
		public string NewName;

		private AudioCaseToll _audioCaseToll;
		private string _name;

		private string[] _names;

		public static void CreateAudionNameBuilder(AudioCaseToll audioCaseToll, string type, string subType = "")
		{
			if(audioCaseToll.Colbeck == null)
				return;

			AudionNameBuilder builder = DisplayWizard<AudionNameBuilder>( Constants.AudionNameBuilder.ButtonName);
			SetNames(builder, type, subType);
			Initialization(audioCaseToll, builder);
		}

		public void OnWizardCreate() =>
			_audioCaseToll.Colbeck?.Invoke(NewName, Case);
		
		private static void SetNames(AudionNameBuilder builder, string type, string subType)
		{
			string[] assets = AssetDatabase.FindAssets($"t:{type} t:{subType}");
			List<string> names = new List<string>(assets.Length);
			names.AddRange(assets.Select(AssetDatabase.GUIDToAssetPath).Select(Path.GetFileNameWithoutExtension));
			builder._names = names.ToArray();
		}

		private static void Initialization(AudioCaseToll audioCaseToll, AudionNameBuilder builder)
		{
			builder._audioCaseToll = audioCaseToll;
			builder.NewName = audioCaseToll.Name;

			string[] guids = AssetDatabase.FindAssets(Constants.AudionNameBuilder.MainAudioCaseTypeToAssets);
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);

			builder.Case = (MainAudioCase) AssetDatabase.LoadAssetAtPath(path, typeof(MainAudioCase));
			builder.OnWizardUpdate();
		}

		public void OnWizardUpdate()
		{
			NewName = NameValidator.TryChangeName(NewName);
			ShowHelpString();
			CheckError();
		}

		private void ShowHelpString() =>
			helpString = Constants.AudionNameBuilder.HelpString;

		private void CheckError()
		{
			string error = "";

			if(_audioCaseToll.Colbeck == null)
				error += Constants.AudionNameBuilder.AudioCaseTollIsNull;
			else if(string.IsNullOrEmpty(_audioCaseToll.Name))
				error += Constants.AudionNameBuilder.NameIsNulL;
			else if(_names.Contains(NewName))
				error += Constants.AudionNameBuilder.NameTakenPleaseSelectAnother;

			errorString = error;
			isValid = errorString.Length == 0;
		}

	}

}