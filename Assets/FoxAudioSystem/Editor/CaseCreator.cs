using System;
using System.Collections.Generic;
using System.IO;
using FoxAudioSystem.Scripts.DataFolder;
using FoxAudioSystem.Scripts.ManagerFolder;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FoxAudioSystem.EditorFolder
{
	public class CaseCreator
	{
		public static void CreateCase<T, W>(string parentFolder, Action<List<T>, W> callback, bool select = false) where T : AudioData where W : AudioDataBase
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

			Action<string, SubCaseAudioGroup> colbeck = (assetName, newCase) => CreateCaseColbeck(parentFolder, newCase, callback, assetName, clipCase, paths, select);
			AudionNameBuilder.CreateAudionNameBuilder(new AudioCaseToll(colbeck, "New"), typeof(W).Name, typeof(T).Name);
		}
		
		private static void CreateCaseColbeck<T, W>(string parentFolder, SubCaseAudioGroup subCaseAudioGroup, Action<List<T>, W> callback, string assetName, List<T> clipCase, Dictionary<string, T> paths, bool select) where T : AudioData where W : AudioDataBase
		{
			string guid = AssetDatabase.CreateFolder(parentFolder, $"{assetName}Folder");
			string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

			foreach(var asset in paths)
				AssetDatabase.MoveAsset(asset.Key, newFolderPath + $"/{asset.Value.name}.asset");

			W data = ScriptableObject.CreateInstance<W>();
			AssetDatabase.CreateAsset(data, newFolderPath + $"/{assetName}.asset");
			AssetDatabase.SaveAssets();

			callback?.Invoke(clipCase, data);
			EditorUtility.SetDirty(data);
			subCaseAudioGroup.Add(data);
			if(select)
			{
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = data;
			}
		}
	}
}