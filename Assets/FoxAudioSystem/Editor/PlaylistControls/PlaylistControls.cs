using FoxAudioSystem.Scripts.PlayersFolder;
using UnityEditor;
using UnityEngine.UIElements;

namespace FoxAudioSystem.Editor.PlaylistControls
{
	[CustomEditor(typeof(PlaylistPLayer))]
	public class PlaylistControls : UnityEditor.Editor
	{
		private VisualElement rootElement;
		private UnityEditor.Editor editor;

		public void OnEnable()
		{
			rootElement = new VisualElement();

			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AudioPaths.EditorPath + "/PlaylistControls/PlaylistControls.uss");
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

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AudioPaths.EditorPath + "/PlaylistControls/PlaylistControls.uxml");
			visualTree.CloneTree(rootElement);

			PlaylistPLayer playlistPLayerTarget = (PlaylistPLayer) target;

			Button generateAudioSourceButton = rootElement.Q<Button>("generate-audio-source");
			generateAudioSourceButton.clicked += () =>
			{
				playlistPLayerTarget.GenerateAudioSource();
			};

			Button previousButton = rootElement.Q<Button>("previous");
			previousButton.clicked += () =>
			{
				playlistPLayerTarget.Previous();
			};
			Button stopButton = rootElement.Q<Button>("stop");
			stopButton.clicked += () =>
			{
				playlistPLayerTarget.Stop();
			};
			Button playButton = rootElement.Q<Button>("play");
			playButton.clicked += () =>
			{
				playlistPLayerTarget.Play();
			};
			Button pauseButton = rootElement.Q<Button>("pause");
			pauseButton.clicked += () =>
			{
				playlistPLayerTarget.TogglePause();
			};
			Button nextButton = rootElement.Q<Button>("next");
			nextButton.clicked += () =>
			{
				playlistPLayerTarget.Next();
			};

			return rootElement;

		}
	}
}