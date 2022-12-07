using FoxAudioSystem.Scripts;
using FoxAudioSystem.Scripts.PlayersFolder;
using UnityEditor;
using UnityEngine.UIElements;

namespace FoxAudioSystem.EditorFolder.PlaylistControls
{
	[CustomEditor(typeof(PlaylistPlayer))]
	public class PlaylistControls : UnityEditor.Editor
	{
		private VisualElement rootElement;
		private UnityEditor.Editor editor;

		public void OnEnable()
		{
			rootElement = new VisualElement();

			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Constants.AudioPaths.EditorPath + "/PlaylistControls/PlaylistControls.uss");
			rootElement.styleSheets.Add(styleSheet);
		}

		public override VisualElement CreateInspectorGUI()
		{
			rootElement.Clear();

			UnityEngine.Object.DestroyImmediate(editor);
			editor = UnityEditor.Editor.CreateEditor(this);
			IMGUIContainer container = new IMGUIContainer(() =>
			{
				if(target)
				{
					OnInspectorGUI();
				}
			});
			rootElement.Add(container);

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Constants.AudioPaths.EditorPath + "/PlaylistControls/PlaylistControls.uxml");
			visualTree.CloneTree(rootElement);

			PlaylistPlayer playlistPlayerTarget = (PlaylistPlayer) target;

			Button generateAudioSourceButton = rootElement.Q<Button>("generate-audio-source");
			generateAudioSourceButton.clicked += () =>
			{
				playlistPlayerTarget.GenerateAudioSource();
			};

			Button previousButton = rootElement.Q<Button>("previous");
			previousButton.clicked += () =>
			{
				playlistPlayerTarget.Previous();
			};
			Button stopButton = rootElement.Q<Button>("stop");
			stopButton.clicked += () =>
			{
				playlistPlayerTarget.Stop();
			};
			Button playButton = rootElement.Q<Button>("play");
			playButton.clicked += () =>
			{
				playlistPlayerTarget.Play();
			};
			Button pauseButton = rootElement.Q<Button>("pause");
			pauseButton.clicked += () =>
			{
				if(playlistPlayerTarget.CanPause)
					playlistPlayerTarget.Pause();
				else
					playlistPlayerTarget.Play();
			};
			Button nextButton = rootElement.Q<Button>("next");
			nextButton.clicked += () =>
			{
				playlistPlayerTarget.Next();
			};

			return rootElement;

		}
	}
}