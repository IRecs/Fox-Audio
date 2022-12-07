using System.IO;
using System.Text;
using UnityEngine;

namespace FoxAudioSystem.Scripts
{
	public static class AudioKeyGenerator
	{
		private static string GetPath()
		{
			FileInfo fileInfo = new FileInfo(Constants.AudioKeyGenerator.PathAudioKey);
			if(fileInfo.Directory == null)
				throw new FileNotFoundException(Constants.AudioKeyGenerator.FileNotFoundException);

			return fileInfo.Directory.FullName + Constants.AudioKeyGenerator.AudioKeyCS ;
		}

		public static void Generate(string[] names)
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Add(Constants.AudioKeyGenerator.Namespace);
			stringBuilder.Add(Constants.AudioKeyGenerator.OpenBracket);
			stringBuilder.Add(Constants.AudioKeyGenerator.ClassName, 2);
			stringBuilder.Add(Constants.AudioKeyGenerator.OpenBracket, 2);

			foreach(string name in names)
			{
				if(name.IndexOf(Constants.AudioKeyGenerator.Spaces) > -1)
				{
					Debug.LogError(Constants.AudioKeyGenerator.SpacesError + $"{name}");
					continue;
				}

				stringBuilder.Add(Constants.AudioKeyGenerator.StringField + $"{name} = nameof({name});", 4);
			}
			stringBuilder.Add(Constants.AudioKeyGenerator.CloseBracket, 2);
			stringBuilder.Add(Constants.AudioKeyGenerator.CloseBracket);

			string text = stringBuilder.ToString();
			Write(text);
		}

		private static void Write(string message)
		{
			string path = GetPath();

			using(FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(message);
				fs.Write(bytes, 0, bytes.Length);
			}
		}

		private static void Add(this StringBuilder stringBuilder, string text, int offset = 0) =>
			stringBuilder.Append(new string(' ', offset) + text + "\n");
	}
}