using System.Text.RegularExpressions;
using FoxAudioSystem.Scripts;

namespace FoxAudioSystem.EditorFolder
{
#pragma warning disable
	public static class NameValidator
	{
		public static string TryChangeName(string name)
		{
			if(HasNoName(name))
				SetNewName(ref name);

			if(!CheckNameForCorrectness(name))
				EditNameToCorrect(ref name);

			return name;
		}
		
		private static bool HasNoName(string name) =>
			string.IsNullOrEmpty(name);
		private static void SetNewName(ref string name) =>
			name = Constants.NameValidator.BaseName;
		private static void EditNameToCorrect(ref string name) =>
			name = Regex.Replace(name, Constants.NameValidator.EditPattern, Constants.NameValidator.ReplacementData);
		private static bool CheckNameForCorrectness(string name) =>
			Regex.Match(name, Constants.NameValidator.ValidatePattern).Success;
	}
}