using System.IO;
using FoxAudioSystem.Scripts.DataFolder;
using UnityEditor;
using UnityEngine;

namespace FoxAudioSystem.Editor
{
	public class AudioTools : MonoBehaviour
	{
		[MenuItem("Assets/FoxAudioSystem/Add SoloAudioClipData", false, 50)]
		public static void CreateSoloAudioClipData() =>
			TryStart<SoloAudioClipData>($"{AudioPaths.DataPath}/SoloAudioClipData/");

		[MenuItem("Assets/FoxAudioSystem/Add PlaylistAudioClipData", false, 50)]
		public static void CreatePlaylistAudioClipData() =>
			TryStart<PlaylistAudioClipData>($"{AudioPaths.DataPath}/PlaylistData/");
		
		[MenuItem("Assets/FoxAudioSystem/Add RandomAudioClipData", false, 50)]
		public static void CreateRandomAudioCliData() =>
			TryStart<RandomAudioClipData>($"{AudioPaths.DataPath}/RandomAudioData/");

		private static void TryStart<T>(string pathNewAsset)where T : AudioData
		{
			foreach(Object obj in Selection.GetFiltered(typeof(AudioClip), SelectionMode.Assets))
			{
				string path = AssetDatabase.GetAssetPath(obj);
				if(string.IsNullOrEmpty(path) || !File.Exists(path))
					continue;
				
				AudioClip audioClip = (AudioClip) AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
				pathNewAsset += $"{audioClip.name}.asset";
				Create<T>(pathNewAsset, audioClip);
				break;
			}
		}
		
		private static void Create<T>(string path, AudioClip audioClip) where T : AudioData
		{
			T data = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(data, path);
			AssetDatabase.SaveAssets();

			data.clip = audioClip;
			EditorUtility.SetDirty(data);

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = data;
		}

	}
}