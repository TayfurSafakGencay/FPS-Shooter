using Actor;
using Audio;
using HelicopterAction;
using Managers.Manager;
using Managers.Manager.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;

namespace Systems.EndGame
{
  public class EndGameSystem : MonoBehaviour
  {
    public static EndGameSystem Instance { get; private set; }
    // Helikopterin inmesi için bütün zombileri öldür yazısı çıkar.
    
    private const string _helicopterAddressable = "Helicopter";
    
    private int _zombieCount;
    
    private Player _player;
    
    private int _chunkCount;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      
      _player = FindObjectOfType<Player>();
      
      _zombieCount = 0;
    }
    
    public void AddZombieCount()
    {
      _zombieCount++;
    }

    public void DeathZombie()
    {
      _zombieCount--;

      if (_chunkCount > 0) return;

      if (_zombieCount == 5)
      {
        CallHelicopter();
      }
      else if (_zombieCount == 0)
      {
        _helicopter.Land();
        HelicopterReached();
      }
      else if (_zombieCount < 5)
      {
        HelicopterReached();
      }
    }
    
    private Helicopter _helicopter;
    
    private async void CallHelicopter()
    {
      AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(_helicopterAddressable);

      await handle.Task;

      if (handle.Status != AsyncOperationStatus.Succeeded) return;
      GameObject helicopterInstance = handle.Result;

      _helicopter = helicopterInstance.GetComponent<Helicopter>();
    }

    public void HelicopterReached()
    {
      if (_zombieCount == 0)
      {
        _player.GetPlayerScreenPanel().DisableInfo();
        return;
      }
      
      string helicopterReachedText = $"Secure the whole area for the helicopter to land safely! {_zombieCount} zombies left.";
      
      _player.GetPlayerScreenPanel().OnInfo(helicopterReachedText);
    }

    private const string _helicopterLandedText = "The helicopter has landed! Proceed to the extraction point now!";
    
    public void HelicopterLanded()
    {
      _player.GetPlayerScreenPanel().OnInfo(_helicopterLandedText);
    }

    [SerializeField]
    private AudioClip _gameEndClip;

    private const float _gameEndSoundDisableDuration = 1f;

    public async void EndGame()
    {
      SettingsManager.Instance.DisableDeviceControls();
      _player.LevelCompleted();

      await Utility.Delay(_gameEndSoundDisableDuration);

      SoundManager.Instance.LevelCompleted(_gameEndSoundDisableDuration);
      await Utility.Delay(_gameEndSoundDisableDuration + 1.5f);

      SoundManager.Instance.PlayMusic(_gameEndClip);
      await Utility.Delay(_gameEndClip.length);
      
      SoundManager.Instance.PlayMusicForEndGame(SoundKey.MainMenuMusic);
    }
    
    public void AddChunkCount()
    {
      _chunkCount++;
    }
    
    public void ChunkActivated()
    {
      _chunkCount--;
    }
  }
}