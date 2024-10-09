using System;
using Audio;
using Managers.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers.Manager
{
  public class SoundManager : ManagerBase
  {
    public static SoundManager Instance { get; private set; }
    
    private SoundsData _soundsData;
    
    [SerializeField]
    private AudioSource _musicAudioSource;
    
    [SerializeField]
    private AudioSource _sfxAudioSource;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }
    
    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChanged);
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

