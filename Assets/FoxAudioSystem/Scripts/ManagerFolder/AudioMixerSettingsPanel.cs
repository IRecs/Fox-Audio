using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoxAudioSystem.Scripts.ManagerFolder
{
    [Serializable]
    public class AudioMixerSettingsPanel
    {
        [SerializeField] private List<AudioMixerSettings> _audioMixerSettings;

        public void Initialization()
        {
            foreach (AudioMixerSettings settings in _audioMixerSettings)
                settings.Initialize();

        }

        private void Mute()
        {
            foreach (AudioMixerSettings audioMixerSetting in _audioMixerSettings)
                audioMixerSetting.Mute();
        }

        private void Unmute()
        {
            foreach (AudioMixerSettings audioMixerSetting in _audioMixerSettings)
                audioMixerSetting.Unmute();
        }
    }
}