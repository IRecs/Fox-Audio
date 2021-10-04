using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Audio
{
  [AddComponentMenu("RFG/Audio/Audio Manager")]
  public class AudioManager : MonoBehaviour
  {
    public List<AudioMixerSettings> AudioMixerSettings;
    private Dictionary<string, Audio> _audioList;
    private Dictionary<string, Playlist> _playList;
    private Dictionary<string, RandomAudio> _randomAudioList;

    private void Awake()
    {
      LoadAudio();
    }

    private void LoadAudio()
    {
      GameObject[] audioGameObjects = GameObject.FindGameObjectsWithTag("Audio");
      _audioList = new Dictionary<string, Audio>();
      _playList = new Dictionary<string, Playlist>();
      _randomAudioList = new Dictionary<string, RandomAudio>();
      foreach (GameObject audioGameObject in audioGameObjects)
      {
        Audio audio = audioGameObject.GetComponent<Audio>();
        if (audio != null)
        {
          _audioList.Add(audio.name, audio);
        }

        Playlist playlist = audioGameObject.GetComponent<Playlist>();
        if (playlist != null)
        {
          _playList.Add(playlist.name, playlist);
        }

        RandomAudio randomAudio = audioGameObject.GetComponent<RandomAudio>();
        if (randomAudio != null)
        {
          _randomAudioList.Add(randomAudio.name, randomAudio);
        }
      }
    }

    private void Start()
    {
      foreach (AudioMixerSettings settings in AudioMixerSettings)
      {
        settings.Initialize();
      }
    }

    public void GenerateAllAudioSources()
    {
      LoadAudio();
      foreach (KeyValuePair<string, Audio> kvp in _audioList)
      {
        Audio audio = kvp.Value;
        audio.AudioData.GenerateAudioSource(audio.gameObject);
        Debug.Log("Generated audio for: " + audio.name);
      }
      foreach (KeyValuePair<string, Playlist> kvp in _playList)
      {
        Playlist playlist = kvp.Value;
        playlist.PlaylistData.AudioList[0].GenerateAudioSource(playlist.gameObject);
        Debug.Log("Generated playlist audio for: " + playlist.name);
      }
      foreach (KeyValuePair<string, RandomAudio> kvp in _randomAudioList)
      {
        RandomAudio randomAudio = kvp.Value;
        randomAudio.RandomAudioData.AudioList[0].GenerateAudioSource(randomAudio.gameObject);
        Debug.Log("Generated random audio for: " + randomAudio.name);
      }
    }

    public void StartAll()
    {
      foreach (KeyValuePair<string, Audio> kvp in _audioList)
      {
        Audio audio = kvp.Value;
        if (audio.AudioData.PlayOnAwake)
        {
          audio.Play();
        }
      }
      foreach (KeyValuePair<string, Playlist> kvp in _playList)
      {
        Playlist playlist = kvp.Value;
        if (playlist.PlaylistData.PlayOnStart)
        {
          playlist.Play();
        }
      }
      foreach (KeyValuePair<string, RandomAudio> kvp in _randomAudioList)
      {
        RandomAudio randomAudio = kvp.Value;
        randomAudio.Start();
      }
    }

    public void StopAll()
    {
      foreach (KeyValuePair<string, Audio> kvp in _audioList)
      {
        Audio audio = kvp.Value;
        audio.Stop();
      }
      foreach (KeyValuePair<string, Playlist> kvp in _playList)
      {
        Playlist playlist = kvp.Value;
        playlist.Stop();
      }
      foreach (KeyValuePair<string, RandomAudio> kvp in _randomAudioList)
      {
        RandomAudio randomAudio = kvp.Value;
        randomAudio.Stop();
      }
    }

    public Audio GetAudio(string name)
    {
      if (!_audioList.ContainsKey(name))
      {
        return null;
      }
      return _audioList[name];
    }

    public Playlist GetPlaylist(string name)
    {
      if (!_playList.ContainsKey(name))
      {
        return null;
      }
      return _playList[name];
    }

    public RandomAudio GetRandomAudio(string name)
    {
      if (!_randomAudioList.ContainsKey(name))
      {
        return null;
      }
      return _randomAudioList[name];
    }

    public void StartAudio(string name)
    {
      Audio audio = GetAudio(name);
      if (audio != null)
      {
        audio.Play();
      }
    }

    public void StopAudio(string name)
    {
      Audio audio = GetAudio(name);
      if (audio != null)
      {
        audio.Stop();
      }
    }

    public void StartPlaylist(string name)
    {
      Playlist playlist = GetPlaylist(name);
      if (playlist != null)
      {
        playlist.Play();
      }
    }

    public void StopPlaylist(string name)
    {
      Playlist playlist = GetPlaylist(name);
      if (playlist != null)
      {
        playlist.Stop();
      }
    }

    public void StartRandomAudio(string name)
    {
      RandomAudio randomAudio = GetRandomAudio(name);
      if (randomAudio != null)
      {
        randomAudio.Start();
      }
    }

    public void StopRandomAudio(string name)
    {
      RandomAudio randomAudio = GetRandomAudio(name);
      if (randomAudio != null)
      {
        randomAudio.Stop();
      }
    }
  }
}