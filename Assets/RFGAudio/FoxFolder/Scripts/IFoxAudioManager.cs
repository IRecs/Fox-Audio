using UnityEngine;

namespace RFG.Audio
{
    public interface IFoxAudioManager
    {
        bool StopAudio(string key);
        bool StopAllAudio(string key);
        bool PlayAudioFollowingTarget(string key, Transform target, bool persist = false);
        bool PlayAudio(string key, Vector3 spawnPosition, bool persist = false);
        void StopAllAudio();
        AudioMixerSettingsPanel Mixer { get; }
    }
}