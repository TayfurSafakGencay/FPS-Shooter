using System;
using System.Threading.Tasks;
using Audio;
using DG.Tweening;
using Managers.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers.Manager
{
  public class SoundManager : ManagerBase
  {
    public static SoundManager Instance { get; private set; }

    private SoundsData _soundsData;

    public SoundsData SoundsData
    {
      get => _soundsData;
      private set
      {
        _soundsData = value;
        if (value == null)
        {
          Deactivate();
        }
        else
        {
          if (ActivatedGameStates.Contains(GameManager.Instance.CurrentGameState))
          {
            Activate();
          }
        }
      }
    }

    [SerializeField]
    private AudioMixer _audioMixer;

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource _musicAudioSource;

    [SerializeField]
    private AudioSource _sfxAudioSource;

    [SerializeField]
    private AudioSource _uiAudioSource;

    [SerializeField]
    private AudioSource _uiAudioSource2;

    [SerializeField]
    private AudioSource _ambienceAudioSource;

    [SerializeField]
    private AudioSource _ambienceAudioSource2;

    [Header("Audio Mixer Groups")]
    private const string _masterMixerGroupKey = "Master";

    private const string _musicMixerGroupKey = "Music";

    private const string _sfxMixerGroupKey = "SFX";

    private const string _uiMixerGroupKey = "UI";

    private const string _ambienceMixerGroupKey = "Ambiance";

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Start()
    {
      SetVolumeFromPlayerPrefs();
    }

    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChanged);

      SetMixerGroupOutputs();
      SetVolumeFromPlayerPrefs();
    }

    private async void OnGameStateChanged()
    {
      switch (GameManager.Instance.CurrentGameState)
      {
        case GameState.LoadGame:
          SoundsData = await LoadAudioData(AudioDataAddressableKey.MainMenuAudioData);
          break;
        case GameState.Game:
          StopMusicFadeOut();
          break;
      }
    }

    public async Task<SoundsData> LoadAudioData(AudioDataAddressableKey audioDataAddressableKey)
    {
      AsyncOperationHandle<SoundsData> asyncOperationHandle = Addressables.LoadAssetAsync<SoundsData>(audioDataAddressableKey.ToString());

      await asyncOperationHandle.Task;

      if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
      {
        asyncOperationHandle.Result.Initialize();
        return asyncOperationHandle.Result;
      }

      Debug.LogWarning("Audio Data could not loaded!");
      return null;
    }

    public void SetVolumeFromPlayerPrefs()
    {
      _audioMixer.SetFloat(_masterMixerGroupKey, PlayerPrefs.GetFloat(_masterMixerGroupKey, 0));
      _audioMixer.SetFloat(_musicMixerGroupKey, PlayerPrefs.GetFloat(_musicMixerGroupKey, 0));
      _audioMixer.SetFloat(_sfxMixerGroupKey, PlayerPrefs.GetFloat(_sfxMixerGroupKey, 0));
      _audioMixer.SetFloat(_uiMixerGroupKey, PlayerPrefs.GetFloat(_uiMixerGroupKey, 0));
      _audioMixer.SetFloat(_ambienceMixerGroupKey, PlayerPrefs.GetFloat(_ambienceMixerGroupKey, 0));
    }

    public void PlayMusic(SoundKey soundKey)
    {
      SoundDTO soundDto = SoundsData.GetSound(soundKey);
      _musicAudioSource.clip = soundDto.AudioClip;
      _musicAudioSource.volume = soundDto.Volume;
      _musicAudioSource.loop = true;
      _musicAudioSource.Play();
    }

    public void PlayMusicForEndGame(SoundKey soundKey, float duration = 4f)
    {
      _musicAudioSource.volume = 0;
      _musicAudioSource.DOFade(1, duration);

      SoundDTO soundDto = SoundsData.GetSound(soundKey);
      _musicAudioSource.clip = soundDto.AudioClip;
      _musicAudioSource.time = 13f;
      _musicAudioSource.loop = true;
      _musicAudioSource.Play();
    }
    
    public void PlayMusic(AudioClip audioClip, float volume = 1)
    {
      _musicAudioSource.clip = audioClip;
      _musicAudioSource.volume = volume;
      _musicAudioSource.loop = false;
      _musicAudioSource.Play();
    }

    public void StopMusic()
    {
      _musicAudioSource.Stop();
    }

    private const float _musicFadeOutDuration = 1.5f;

    public void StopMusicFadeOut(float duration = _musicFadeOutDuration)
    {
      _musicAudioSource.DOFade(0, duration).OnComplete(StopMusic);
    }

    public void PlayUISound(SoundKey soundKey, float volume = 1, int uiAudioSourceIndex = 1)
    {
      SoundDTO soundDto = SoundsData.GetSound(soundKey);

      switch (uiAudioSourceIndex)
      {
        case 1:
          PlayUISound(ref _uiAudioSource, soundDto, volume);
          break;
        case 2:
          PlayUISound(ref _uiAudioSource2, soundDto, volume);
          break;
      }
    }

    private void PlayUISound(ref AudioSource audioSource, SoundDTO soundDto, float volume = 1)
    {
      audioSource.clip = soundDto.AudioClip;
      audioSource.volume = soundDto.Volume * volume;

      audioSource.Play();
    }

    public void PlayAmbienceSound(SoundKey soundKey, float volume = 1, int ambienceAudioSourceIndex = 1, bool loop = false)
    {
      SoundDTO soundDto = SoundsData.GetSound(soundKey);

      switch (ambienceAudioSourceIndex)
      {
        case 1:
          PlayAmbienceSound(ref _ambienceAudioSource, soundDto, volume, loop);
          break;
        case 2:
          PlayAmbienceSound(ref _ambienceAudioSource2, soundDto, volume, loop);
          break;
      }
    }

    private void PlayAmbienceSound(ref AudioSource audioSource, SoundDTO soundDto, float volume = 1, bool loop = false)
    {
      audioSource.clip = soundDto.AudioClip;
      audioSource.volume = soundDto.Volume * volume;
      audioSource.loop = loop;

      audioSource.Play();
    }

    public void PlaySound(SoundKey soundKey, ref AudioSource audioSource, float volume = 1)
    {
      SoundDTO soundDto = SoundsData.GetSound(soundKey);
      audioSource.clip = soundDto.AudioClip;
      audioSource.volume = soundDto.Volume * volume;

      audioSource.Play();
    }

    public void PlayOneShot(SoundKey soundKey, AudioSource audioSource, float volume = 1)
    {
      audioSource.PlayOneShot(SoundsData.GetSound(soundKey).AudioClip, volume);
    }

    public void PlaySoundEffect(SoundKey soundKey, float volume = 1)
    {
      _sfxAudioSource.PlayOneShot(SoundsData.GetSound(soundKey).AudioClip, volume);
    }

    public AudioMixerGroup GetMixerGroupToAudioSourceForSFX()
    {
      return _sfxAudioSource.outputAudioMixerGroup;
    }

    private void SetMixerGroupOutputs()
    {
      _musicAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_musicMixerGroupKey)[0];
      _sfxAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_sfxMixerGroupKey)[0];

      _uiAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_uiMixerGroupKey)[0];
      _uiAudioSource2.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_uiMixerGroupKey)[0];

      _ambienceAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_ambienceMixerGroupKey)[0];
      _ambienceAudioSource2.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(_ambienceMixerGroupKey)[0];
    }

    #region Set Volume

    private const float _minDb = -80;

    private const float _maxDb = 0;

    public void SetMasterVolume(float normalizedValue)
    {
      float volume = CalculateVolume(normalizedValue);
      
      _audioMixer.SetFloat(_masterMixerGroupKey, volume);
      PlayerPrefs.SetFloat(_masterMixerGroupKey, volume);
    }

    public void SetMusicVolume(float normalizedValue)
    {
      float volume = CalculateVolume(normalizedValue);
      
      _audioMixer.SetFloat(_musicMixerGroupKey, volume);
      PlayerPrefs.SetFloat(_musicMixerGroupKey, volume);
    }

    public void SetSFXVolume(float normalizedValue)
    {
      float volume = CalculateVolume(normalizedValue);
      
      _audioMixer.SetFloat(_sfxMixerGroupKey, volume);
      PlayerPrefs.SetFloat(_sfxMixerGroupKey, volume);
    }

    public void SetUIVolume(float normalizedValue)
    {
      float volume = CalculateVolume(normalizedValue);
      
      _audioMixer.SetFloat(_uiMixerGroupKey, volume);
      PlayerPrefs.SetFloat(_uiMixerGroupKey, volume);
    }

    public void SetAmbienceVolume(float normalizedValue)
    {
      float volume = CalculateVolume(normalizedValue);
      
      _audioMixer.SetFloat(_ambienceMixerGroupKey, volume);
      PlayerPrefs.SetFloat(_ambienceMixerGroupKey, volume);
    }
    
    public void DecreaseAllVolume(float duration)
    {
      _audioMixer.DOSetFloat(_masterMixerGroupKey, _minDb, duration);
    }

    public void LevelCompleted(float duration)
    {
      _audioMixer.DOSetFloat(_ambienceMixerGroupKey, _minDb, duration);
      _audioMixer.DOSetFloat(_sfxMixerGroupKey, _minDb, duration);
      _audioMixer.DOSetFloat(_uiMixerGroupKey, _minDb, duration);
    }

    private float CalculateVolume(float normalizedValue)
    {
      return Mathf.Lerp(_minDb, _maxDb, normalizedValue);
    }

    #endregion
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
    MainMenuAudioData = 0,
    GameAudioData = 1,
  }
}