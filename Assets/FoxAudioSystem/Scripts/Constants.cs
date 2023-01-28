namespace FoxAudioSystem.Scripts
{
	public static class Constants
	{
		public static class AudionNameBuilder
		{
			public const string MainAudioCaseTypeToAssets = "t:MainAudioCase";
			public const string ButtonName = "Create Audion";
			public const string AudioCaseTollIsNull = "AudioCaseToll is null!\n";
			public const string NameIsNulL = "Name is nul!l\n";
			public const string NameTakenPleaseSelectAnother = "Name taken, please select another!\n";
			public const string HelpString = "Enter the desired name for the new resource."
			                                 + "The name is automatically adjusted to the required format\n";
		}
		
		public static class CreateAssetMenuPaths
		{
			public const string PlaylistDataCase  = "FoxAudioSystem/Audio/Data/Playlist/Playlist Case";
			public const string CaseIsNull = "Case is null!\n";
			public const string HelpString = "Choose which group you want to add\n";
		}

		public static class CreateAssetMenuNames
		{
			public const string PlaylistDataCase  = "new PlaylistDataCase ";
			public const string CaseIsNull = "Case is null!\n";
			public const string HelpString = "Choose which group you want to add\n";
		}
		
		public static class CaseSelectorBuilder
		{
			public const string ButtonName = "Create Audion";
			public const string CaseIsNull = "Case is null!\n";
			public const string HelpString = "Choose which group you want to add\n";
		}

		public static class NameValidator
		{
			public const string BaseName = "New";
			public const string ValidatePattern = "^[A-Z][a-zA-Z][_]*$";
			public const string EditPattern = @"[^0-9a-zA-Z_]+";
			public const string ReplacementData = "";

		}

		public static class AudioPaths
		{
			public const string EditorPath = "Assets/FoxAudioSystem/Editor";
			public const string DataPath = "Assets/FoxAudioSystem/Data";
			public const string PlaylistData = DataPath + "/PlaylistData";
			public const string SoloAudioClipData = DataPath + "/SoloAudioClipData";
			public const string RandomAudioData = DataPath + "/RandomAudioData";
		}

		public class AudioKeyGenerator
		{
			public const string PathAudioKey = "Assets\\FoxAudioSystem\\Scripts\\CoreFolde" + AudioKeyCS;
			public const string AudioKeyCS = "\\AudioKey.cs";
			public const string Namespace = "namespace FoxAudioSystem.Editor";
			public const string ClassName = "public static class AudioKey";
			public const string StringField = "public const string ";
			public const string SpacesError = "Spaces in the name are not allowed. ";
			public const char Spaces = ' ';
			public const string FileNotFoundException = "Directory" + PathAudioKey + "is not found";
			public const string OpenBracket = "{";
			public const string CloseBracket = "}";
		}
	}
}