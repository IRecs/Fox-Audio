namespace RFG.Audio
{
  public interface IAudio
  {
    void GenerateAudioSource();
    void Play();
    void Stop();
    void Persist(bool persist);
  }
}