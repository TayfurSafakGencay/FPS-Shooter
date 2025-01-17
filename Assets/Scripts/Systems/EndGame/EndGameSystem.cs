using Actor;
using Audio;
using DayCycle;
using HelicopterAction;
using Managers.Manager;
using Managers.Manager.Settings;
using PostProcess;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;

namespace Systems.EndGame
{
  public class EndGameSystem : MonoBehaviour
  {
    public static EndGameSystem Instance { get; private set; }
    
    private const string _helicopterAddressable = "Helicopter";
    
    private int _zombieCount;

    private int _zombieSpawnerCount;
    
    private int _spawnedZombieCount;
    
    private Player _player;

    public bool EndGameChecker;
    
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      
      _player = FindObjectOfType<Player>();
      
      _zombieCount = 0;
      _zombieSpawnerCount = 0;
    }
    
    public void AddZombieCount()
    {
      _spawnedZombieCount++;
      _zombieCount++;
      
      print($"Zombie Spawned! - Spawned Zombie Count: {_spawnedZombieCount}, Zombie Spawner Count: {_zombieSpawnerCount}, Zombie Count: {_zombieCount}");
    }
    
    public void AddZombieSpawner()
    {
      _zombieSpawnerCount++;
    }

    public void DeathZombie()
    {
      _zombieCount--;
      
      print($"Zombie Dead! - Spawned Zombie Count: {_spawnedZombieCount}, Zombie Spawner Count: {_zombieSpawnerCount}, Zombie Count: {_zombieCount}");

      if (_spawnedZombieCount < _zombieSpawnerCount)
      {
        print("Henüz tüm zombiler spawn olmadı!");
        return;
      }

      if (_zombieCount < 10 && !_helicopterCalled)
      {
        CallHelicopter();
        HelicopterReached();
      }
      else if (_zombieCount == 0)
      {
        _helicopter.Land();
        
        string helicopterLandingText = "You’ve secured the entire area, and the helicopter is now landing. Proceed to the extraction point immediately!";
      
        _player.GetPlayerScreenPanel().OnInfo(helicopterLandingText);
      }
      else
      {
        HelicopterReached();
      }
    }
    
    private Helicopter _helicopter;
    
    private bool _helicopterCalled;
    
    private async void CallHelicopter()
    {
      if (_helicopterCalled) return;
      _helicopterCalled = true;
      
      AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(_helicopterAddressable);

      await handle.Task;

      if (handle.Status != AsyncOperationStatus.Succeeded) return;
      GameObject helicopterInstance = handle.Result;

      _helicopter = helicopterInstance.GetComponent<Helicopter>();
    }

    public void HelicopterReached()
    {
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
      DayCycleGeneralOperations.EndGame();
      DayCycleSystem.EndGame();
      
      EndGameChecker = true;
      
      SettingsManager.Instance.DisableDeviceControls();
      _player.LevelCompleted();

      await Utility.Delay(_gameEndSoundDisableDuration);

      SoundManager.Instance.LevelCompleted(_gameEndSoundDisableDuration);
      await Utility.Delay(_gameEndSoundDisableDuration + 1.5f);

      SoundManager.Instance.PlayMusic(_gameEndClip);
      await Utility.Delay(_gameEndClip.length + 1f);
      
      VignetteEffect.GameCompleted();
      SoundManager.Instance.PlayMusicForEndGame(SoundKey.MainMenuMusic);
    }
  }
}