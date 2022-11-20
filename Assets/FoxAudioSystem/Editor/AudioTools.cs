using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FoxAudioSystem.Scripts.DataFolder;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FoxAudioSystem.EditorFolder
{
	public class AudioTools : MonoBehaviour
	{
		private const string Random = "Assets/FoxAudioSystem/Random";
		private const string RandomAudioClipData = Random + "/Create RandomAudioClipData";
		private const string RandomAudioDataCase = Random + "/Create RandomAudioDataCase";

		private const string Playlist = "Assets/FoxAudioSystem/Playlist";
		private const string PlaylistAudioClipData = Playlist + "/Create PlaylistAudioClipData";
		private const string PlaylistAudioClipCase = Playlist + "/Create PlaylistAudioClipCase";

		private const string SoloAudioClipData = "Assets/FoxAudioSystem/Solo/Create SoloAudioClipData";

		[MenuItem(RandomAudioDataCase, false, 50)]
		public static void CreateRandomAudioDataCase()
		{
			Action<List<RandomAudioClipData>, RandomAudioDataCase> callback = (list, dataCase) => dataCase.audioList = list;
			CreateCase(AudioPaths.RandomAudioData, callback);
		}
		
		[MenuItem(PlaylistAudioClipCase, false, 50)]
		public static void CreatePlaylistAudioClipCase()
		{
			Action<List<PlaylistAudioClipData>, PlaylistDataCase> callback = (list, dataCase) => dataCase.audioList = list;
			CreateCase(AudioPaths.PlaylistData, callback);
		}


		[MenuItem(SoloAudioClipData, false, 50)]
		public static void CreateSoloAudioClipData() =>
			TryStart<SoloAudioClipData>(AudioPaths.SoloAudioClipData);

		[MenuItem(PlaylistAudioClipData, false, 50)]
		public static void CreatePlaylistAudioClipData() =>
			TryStart<PlaylistAudioClipData>(AudioPaths.PlaylistData);

		[MenuItem(RandomAudioClipData, false, 50)]
		public static void CreateRandomAudioCliData() =>
			TryStart<RandomAudioClipData>(AudioPaths.RandomAudioData);

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

				string assetPath = pathNewAsset +  $"/{audioClip.name}.asset";
				Create<T>(assetPath, audioClip);
			}
		}

		private static bool NeedChangeName( ref string name)
		{
			if(!Regex.Match(name, "^[A-Z][a-zA-Z][_]*$").Success)
			{
				name = Regex.Replace(name, @"[^0-9a-zA-Z_]+", "");
				return true;
			}

			return false;
		}

		private static void Create<T>(string path, AudioClip audioClip) where T : AudioData
		{
			T data = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(data, path);
			AssetDatabase.SaveAssets();

			data.clip = audioClip;
			EditorUtility.SetDirty(data);

			Selection.activeObject = data;
			EditorUtility.FocusProjectWindow();
		}

		private static void CreateCase<T, W>(string parentFolder, Action<List<T>, W> callback) where T : AudioData where W : AudioDataBase
		{
			List<T> clipCase = new List<T>();
			Dictionary<string, T> paths = new Dictionary<string, T>();

			foreach(Object obj in Selection.GetFiltered(typeof(T), SelectionMode.Assets))
			{
				string path = AssetDatabase.GetAssetPath(obj);
				if(string.IsNullOrEmpty(path) || !File.Exists(path))
					continue;
				T clipData = (T) AssetDatabase.LoadAssetAtPath(path, typeof(T));
				paths.Add(path, clipData);
				clipCase.Add(clipData);
			}

			if(clipCase.Count <= 0)
				return;

			string guid = AssetDatabase.CreateFolder(parentFolder, $"{clipCase[0].name}Folder");
			string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

			W data = ScriptableObject.CreateInstance<W>();
			AssetDatabase.CreateAsset(data, newFolderPath + $"/{clipCase[0].name}Case.asset");
			AssetDatabase.SaveAssets();

			callback?.Invoke(clipCase, data);
			EditorUtility.SetDirty(data);

			foreach(var asset in paths)
				AssetDatabase.MoveAsset(asset.Key, newFolderPath + $"/{asset.Value.name}.asset");

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = data;
		}
	}

}