using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEditor;
using UnityEngine;

#pragma warning disable
namespace FoxAudioSystem.EditorFolder
{
	public class AudionNameBuilder : SubCaseSelectorBuilder
	{
		public string NewName;

		private AudioCaseToll _audioCaseToll;
		private string _name;

		private string[] _names;

		private static void SetNames(AudionNameBuilder builder, string type, string subType)
		{
			string[] assets = AssetDatabase.FindAssets($"t:{type} t:{subType}");
			List<string> names = new List<string>(assets.Length);
			foreach(string guid in assets)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				int lastPoint = 0;
				for( int i = 0; i < path.Length; i++ )
				{
					if(path[i] == '/')
						lastPoint = i;
				}
				path = path.Remove(0, lastPoint + 1);
				path = path.Remove(path.Length - 6);
				Debug.Log(path);
				names.Add(path);
			}
			builder._names = names.ToArray();
		}

		public static void CreateAudionNameBuilder(AudioCaseToll audioCaseToll, string type, string subType = "")
		{
			if(audioCaseToll.Colbeck == null)
				return;

			AudionNameBuilder builder = DisplayWizard<AudionNameBuilder>("Create Audion");
			SetNames(builder, type, subType);
			Initialization(audioCaseToll, builder);
		}

		private static void Initialization(AudioCaseToll audioCaseToll, AudionNameBuilder builder)
		{
			builder._audioCaseToll = audioCaseToll;
			builder.NewName = audioCaseToll.Name;

			string[] guids = AssetDatabase.FindAssets("t:MainAudioCase");
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);

			builder.Case = (MainAudioCase)AssetDatabase.LoadAssetAtPath(path, typeof(MainAudioCase));
			builder.OnWizardUpdate();
		}

		public void OnWizardUpdate()
		{
			TryChangeName();
			ShowHelpString();
			CheckError();
		}

		private void TryChangeName()
		{
			NewName = NeedChangeName(NewName);
		}

		private static string NeedChangeName(string name)
		{
			if(string.IsNullOrEmpty(name))
				return "New";

			if(!Regex.Match(name, "^[A-Z][a-zA-Z][_]*$").Success)
				name = Regex.Replace(name, @"[^0-9a-zA-Z_]+", "");

			return name;
		}

		private void ShowHelpString()
		{
			helpString = "Enter the desired name for the new resource."
			             + "The name is automatically adjusted to the required format\n";
		}

		private void CheckError()
		{
			string error = "";

			if(_audioCaseToll.Colbeck == null)
				error += "AudioCaseToll is null!\n";
			else if(string.IsNullOrEmpty(_audioCaseToll.Name))
				error += "Name is nul!l\n";
			else if(_names.Contains(NewName))
				error += "Name taken, please select another!\n";

			errorString = error;
			isValid = errorString.Length == 0;
		}

		public void OnWizardCreate() =>
			_audioCaseToll.Colbeck?.Invoke(NewName, Case);
	}

}