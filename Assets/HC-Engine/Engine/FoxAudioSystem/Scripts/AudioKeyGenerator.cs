using System.IO;
using System.Text;
using UnityEngine;

namespace FoxAudioSystem
{
	public static class AudioKeyGenerator
	{
		public const string PathAudioKey = "Assets\\FoxAudioSystem\\Scripts\\CoreFolder\\AudioKey.cs";

		private static string GetPath()
		{
			FileInfo fileInfo = new FileInfo(PathAudioKey);
			return fileInfo.Directory.FullName + "\\AudioKey.cs";
		}

		public static void Generate(string[] names)
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Add("namespace FoxAudioSystem.Editor");
			stringBuilder.Add("{");
			stringBuilder.Add("  public static class AudioKey");
			stringBuilder.Add("  {");

			foreach(string name in names)
			{
				if(name.IndexOf(' ') > -1)
				{
					Debug.LogError($" Spaces in the name are not allowed. {name}");
					continue;
				}

				stringBuilder.Add($"    public const string {name} = nameof({name});");
			}
			stringBuilder.Add("  }");
			stringBuilder.Add("}");

			string text = stringBuilder.ToString();
			Write(text);
		}

		private static void Write(string message)
		{
			string path = GetPath();

			using(FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				var bytes = Encoding.UTF8.GetBytes(message);
				fs.Write(bytes, 0, bytes.Length);
			}
		}

		private static void Add(this StringBuilder stringBuilder, string text) =>
			stringBuilder.Append(text + "\n");
	}
}