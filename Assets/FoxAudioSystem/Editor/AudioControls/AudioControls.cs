using FoxAudioSystem.Scripts.ExtensionFolder;
using FoxAudioSystem.Scripts.PlayersFolder;
using UnityEditor;
using UnityEngine.UIElements;

namespace FoxAudioSystem.EditorFolder.AudioControls
{
	[CustomEditor(typeof(SoloAudioPlayer))]
	public class AudioControls : UnityEditor.Editor
	{
		private VisualElement rootElement;
		private UnityEditor.Editor editor;

		public void OnEnable()
		{
			rootElement = new VisualElement();

			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AudioPaths.EditorPath + "/AudioControls/AudioControls.uss");
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

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AudioPaths.EditorPath + "/AudioControls/AudioControls.uxml");
			visualTree.CloneTree(rootElement);

			SoloAudioPlayer soloAudioPlayerTarget = (SoloAudioPlayer) target;

			Button generateAudioSourceButton = rootElement.Q<Button>("generate-audio-source");
			generateAudioSourceButton.clicked += () =>
			{
				soloAudioPlayerTarget.Data.DataObject.GenerateAudioSource(soloAudioPlayerTarget.gameObject);
			};

			Button stopButton = rootElement.Q<Button>("stop");
			stopButton.clicked += () =>
			{
				soloAudioPlayerTarget.Stop();
			};
			Button playButton = rootElement.Q<Button>("play");
			playButton.clicked += () =>
			{
				soloAudioPlayerTarget.Play();
			};
			Button pauseButton = rootElement.Q<Button>("pause");
			pauseButton.clicked += () =>
			{
				soloAudioPlayerTarget.Pause();
			};

			return rootElement;

		}
	}
}