using FoxAudioSystem.Scripts.PlayersFolder;

namespace FoxAudioSystem.Scripts.CoreFolder
{
	public class ControlledAudioResource
	{
		public readonly IAudioPlayer audioPlayer;
		public string Key => audioPlayer.Name;
		public string ID => audioPlayer.ID;

		public ControlledAudioResource(IAudioPlayer audioPlayer) =>
			this.audioPlayer = audioPlayer;
	}
}