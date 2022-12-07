using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FoxAudioSystem.Scripts;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FoxAudioSystem.EditorFolder
{
	public class AudioTools
	{
		private const string RandomAudioClipData = Data + "Random ClipData";
		private const string RandomAudioClipDataToCase = Data + "Random ClipData to case";
		private const string RandomAudioDataCase = Data + "Random Case";

		private const string PlaylistAudioClipData = Data + "Playlist ClipData";
		private const string PlaylistAudioClipDataToCase = Data + "Playlist ClipData to case";
		private const string PlaylistAudioClipCase = Data + "Playlist Case";

		private const string SoloAudioClipData = Data + "Solo ClipData";
		private const string SoloAudioClipDataToCase = Data + "Solo ClipData to case";

		private const string Data = "Assets/FoxAudioSystem/Data/";
	
		#region Validate
		[MenuItem(SoloAudioClipData, true)]
		[MenuItem(SoloAudioClipDataToCase, true)]
		[MenuItem(PlaylistAudioClipData, true)]
		[MenuItem(PlaylistAudioClipDataToCase, true)]
		[MenuItem(RandomAudioClipData, true)]
		[MenuItem(RandomAudioClipDataToCase, true)]
		public static bool ValidateAudioClip() =>
			Check<AudioClip>();
		#endregion

		#region Solo
		[MenuItem(SoloAudioClipData, false, 0)]
		public static bool CreateSoloAudioClipData()
		{
			TryStart<SoloAudioClipData>(Constants.AudioPaths.SoloAudioClipData);
			return true;
		}
		[MenuItem(SoloAudioClipDataToCase, false, 0)]
		public static void CreateSoloAudioClipDataToCase() =>
			TryStartAndAddToCase<SoloAudioClipData, SubCaseAudioGroup>(SubCaseSelectorBuilder.CreateWizard());
		#endregion

		#region Playlist
		[MenuItem(PlaylistAudioClipData, false, 20)]
		public static void CreatePlaylistAudioClipData() =>
			TryStart<PlaylistAudioClipData>(Constants.AudioPaths.PlaylistData);

		[MenuItem(PlaylistAudioClipDataToCase, false, 20)]
		public static void CreatePlaylistAudioClipDataToCase() =>
			TryStartAndAddToCase<PlaylistAudioClipData, PlaylistDataCase>(PlaylistDataCaseSelectorBuilder.CreateWizard());

		[MenuItem(PlaylistAudioClipCase, false, 20)]
		public static void CreatePlaylistAudioClipCase() =>
			CreatePlaylistAudioClipCase(true);

		private static void CreatePlaylistAudioClipCase(bool select)
		{
			Action<List<PlaylistAudioClipData>, PlaylistDataCase> callback = (list, dataCase) => dataCase.SetAudioList(list);
			CaseCreator.CreateCase(Constants.AudioPaths.PlaylistData, callback, select);
		}
		#endregion

		#region Random
		[MenuItem(RandomAudioClipData, false, 40)]
		public static void CreateRandomAudioCliData() =>
			TryStart<RandomAudioClipData>(Constants.AudioPaths.RandomAudioData);

		[MenuItem(RandomAudioClipDataToCase, false, 40)]
		public static void CreateRandomAudioCliDataToCase() =>
			TryStartAndAddToCase<RandomAudioClipData, RandomAudioDataCase>(RandomAudioDataCaseSelectorBuilder.CreateWizard());

		[MenuItem(RandomAudioDataCase, false, 40)]
		public static void CreateRandomAudioDataCase() =>
			CreateRandomAudioDataCase(true);

		private static void CreateRandomAudioDataCase(bool select)
		{
			Action<List<RandomAudioClipData>, RandomAudioDataCase> callback = (list, dataCase) => dataCase.audioList = list;
			CaseCreator.CreateCase(Constants.AudioPaths.RandomAudioData, callback, select);
		}
		#endregion

		#region Data
		private static void TryStartAndAddToCase<T, W>(CaseSelectorBuilder<W> caseSelectorBuilder) where T : AudioData where W : ScriptableObject, ICase
		{
			if(AssetDatabase.FindAssets($"t:{typeof(W).Name}").Length == 0)
			{
				if(typeof(T).Name.Equals(typeof(PlaylistAudioClipData).Name))
					CreatePlaylistAudioClipCase(false);

				if(typeof(T).Name.Equals(typeof(RandomAudioClipData).Name))
					CreateRandomAudioDataCase(false);
			}

			caseSelectorBuilder.Close += newCase => AfterSelectCase<T>(newCase);
		}

		private static void AfterSelectCase<T>(ICase newCase) where T : AudioData
		{
			string pathNewAsset =  AssetDatabase.GetAssetPath((Object)newCase);
			int count = $"{((Object) newCase).name}.asset".Length;
			pathNewAsset =  pathNewAsset.Remove(pathNewAsset.Length - count);
			
			foreach(Object obj in Selection.GetFiltered(typeof(AudioClip), SelectionMode.Assets))
			{
				string path = AssetDatabase.GetAssetPath(obj);
				if(string.IsNullOrEmpty(path) || !File.Exists(path))
					continue;

				AudioClip audioClip = (AudioClip) AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
				string name = audioClip.name;

				if(NeedChangeName(ref name))
					AssetDatabase.RenameAsset(path, name);

				string assetPath = pathNewAsset + $"{audioClip.name}.asset";
				T newAsset = Create<T>(assetPath, audioClip);

				if(newCase != null)
					newCase.Add<T>(newAsset);
			}
		}

		private static void TryStart<T>(string pathNewAsset) where T : AudioData
		{
			foreach(Object obj in Selection.GetFiltered(typeof(AudioClip), SelectionMode.Assets))
			{
				string path = AssetDatabase.GetAssetPath(obj);
				if(string.IsNullOrEmpty(path) || !File.Exists(path))
					continue;

				AudioClip audioClip = (AudioClip) AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
				string name = audioClip.name;

				if(NeedChangeName(ref name))
					AssetDatabase.RenameAsset(path, name);

				string assetPath = pathNewAsset + $"/{audioClip.name}.asset";
				T newAsset = Create<T>(assetPath, audioClip);
			}
		}

		private static T Create<T>(string path, AudioClip audioClip) where T : AudioData
		{
			T data = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(data, path);
			AssetDatabase.SaveAssets();

			data.clip = audioClip;
			EditorUtility.SetDirty(data);

			Selection.activeObject = data;
			EditorUtility.FocusProjectWindow();
			return data;
		}
		#endregion

		#region Case
		private static bool NeedChangeName(ref string name)
		{
			if(!Regex.Match(name, "^[A-Z][a-zA-Z][_]*$").Success)
			{
				name = Regex.Replace(name, @"[^0-9a-zA-Z_]+", "");
				return true;
			}

			return false;
		}
		
		private static bool Check<T>()
		{
			Object[] objects = Selection.GetFiltered(typeof(T), SelectionMode.Assets);
			return objects.Length != 0;
		}
		#endregion

	}
}