namespace RFG.Audio
{
	public class ControlledAudioResource
	{
		public readonly IAudio Audio;
		public string Key => Audio.Name;
		public string ID => Audio.ID;

		public ControlledAudioResource(IAudio audio) =>
			Audio = audio;
	}
}