using System;
using Audio;
using Managers.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Managers.Manager
{
  public class SoundManager : ManagerBase
  {
    public static SoundManager Instance { get; private set; }
    
    private SoundsData _soundsData;

    [SerializeField]
    private AudioMixer _audioMixer;
    
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource _musicAudioSource;
    
    [SerializeField]
    private AudioSource _sfxAudioSource;

    [SerializeField]
    private AudioSource _uiAudioSource;

    [Header("Audio Mixer Groups")]
    private const string _masterMixerGroupKey = "Master";
    
    private const string _musicMixerGroupKey = "Music";

    private const string _sfxMixerGroupKey = "SFX";
    
    private const string _uiMixerGroupKey = "UI";
    
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }
    
    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChanged);
      
      SetMixerGroupOutputs();
    }
    
    private void OnGameStateChanged()
    {
      if (ActivatedGameStates.Contains(GameManager.Instance.CurrentGameState))
      {
        Activate();
      }
      
      switch (GameManager.Instance.CurrentGameState)
      {
        case GameState.MainMenu:
          LoadAudioData(AudioDataAddressableKey.MainMenuAudioData);
          break;
      }
    }
    
    private void LoadAudioData(AudioDataAddressableKey audioDataAddressableKey)
    {
      Addressables.LoadAssetAsync<SoundsData>(audioDataAddressableKey.ToString()).Completed += handle =>
      {
        _soundsData = handle.Result;
        _soundsData.Initialize();
      };
    }

    private void SetMixerGroupOutputs()
    {
      _musicAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_musicMixerGroupKey)[0];
      _sfxAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_sfxMixerGroupKey)[0];
      _uiAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_uiMixerGroupKey)[0];
    }

    public void PlayMusic(SoundKey soundKey)
    {
      SoundDTO soundDto = _soundsData.GetSound(soundKey);
      _musicAudioSource.clip = soundDto.AudioClip;
      _musicAudioSource.volume = soundDto.Volume;
      _musicAudioSource.loop = true;
      _musicAudioSource.Play();
    }
    
    public void PlaySound(SoundKey soundKey, float volume = 1)
    {
      SoundDTO soundDto = _soundsData.GetSound(soundKey);
    }
    
    public void PlaySound(SoundKey soundKey, ref AudioSource audioSource, float volume = 1)
    {
      SoundDTO soundDto = _soundsData.GetSound(soundKey);
      audioSource.clip = soundDto.AudioClip;
      audioSource.volume = soundDto.Volume * volume;

      audioSource.Play();
    }

    public void PlayOneShot(SoundKey soundKey, AudioSource audioSource, float volume = 1)
    {
      audioSource.PlayOneShot(_soundsData.GetSound(soundKey).AudioClip, volume);
    }
    
    public void AttachMixerGroupToAudioSourceForSFX(ref AudioSource audioSource)
    {
      audioSource.outputAudioMixerGroup = _sfxAudioSource.outputAudioMixerGroup;
    }
    
    private const float _minDb = -80;
    
    private const float _maxDb = 0;
    public void SetMasterVolume(float normalizedValue)
    {
      _audioMixer.SetFloat(_masterMixerGroupKey, CalculateVolume(normalizedValue));
    }
    
    public void SetMusicVolume(float normalizedValue)
    {
      _audioMixer.SetFloat(_musicMixerGroupKey, CalculateVolume(normalizedValue));
    }
    
    public void SetSFXVolume(float normalizedValue)
    {
      _audioMixer.SetFloat(_sfxMixerGroupKey, CalculateVolume(normalizedValue));
    }
    
    public void SetUIVolume(float normalizedValue)
    {
      _audioMixer.SetFloat(_uiMixerGroupKey, CalculateVolume(normalizedValue));
    }

    private float CalculateVolume(float normalizedValue)
    {
      return Mathf.Lerp(_minDb, _maxDb, normalizedValue);
    }
  }
  
  [Serializable]
  public record SoundDTO
  {
    public string Name;
    
    public SoundKey SoundKey;
   
    public AudioClip AudioClip;
    
    [Range(0, 1)]
    public float Volume = 1f;
  }

  public enum AudioDataAddressableKey
  {
    MainMenuAudioData,
  }
}

