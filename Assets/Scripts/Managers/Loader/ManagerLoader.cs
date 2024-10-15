using System;
using System.Collections.Generic;
using Managers.Base;
using Managers.Manager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers.Loader
{
  public class ManagerLoader : MonoBehaviour
  {
    public static ManagerLoader Instance { get; private set; }
    
    [Header("Active Managers For Load Game")]
    [SerializeField] private List<ManagerKey> _activeManagersForLoadGame;
    
    [Space(25)]
    [Header("Active Managers For Load Assets")]
    [SerializeField] private List<ManagerKey> _activeManagersForLoadAssets;
    
    private readonly List<ManagerKey> _initializedManagers = new();
    
    private readonly List<ManagerKey> _activatedManagers = new();

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);

      LoadManagersAccordingToGameState();
      
      DontDestroyOnLoad(this);
    }
    
    private async void LoadManagersAccordingToGameState()
    {
      ManagerKey[] managerKeys = (ManagerKey[])Enum.GetValues(typeof(ManagerKey));
      
      foreach (ManagerKey key in managerKeys)
      {
        if (_initializedManagers.Contains(key)) continue;
        
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(key.ToString(), transform);

        if (key == ManagerKey.GameManager)
        {
          await asyncOperationHandle.Task;
        }

        asyncOperationHandle.Completed += handle =>
        {
          GameObject manager = handle.Result;
          manager.GetComponent<ManagerBase>().Initialize();
          _initializedManagers.Add(key);

          if (managerKeys.Length == _initializedManagers.Count)
          {
            AllManagersInitialized();
          }
        };
      }
    }
    
    private void AllManagersInitialized()
    {
      GameManager.Instance.ChangeGameState(GameState.LoadGame);
    }
    
    public void ActivateManager(ManagerKey managerKey)
    {
      if (CheckManagerActivated(managerKey)) return;
      
      _activatedManagers.Add(managerKey);

      if (_activatedManagers.Count == _activeManagersForLoadGame.Count)
      {
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
      }
      else if (_activatedManagers.Count == _activeManagersForLoadAssets.Count + _activeManagersForLoadGame.Count)
      {
        GameManager.Instance.ChangeGameState(GameState.Game);
      }
    }
    
    public void DeactivateManager(ManagerKey managerKey)
    {
      if (CheckManagerActivated(managerKey))
      {
        _activatedManagers.Remove(managerKey);
      }
    }
    
    public bool CheckManagerActivated(ManagerKey managerKey)
    {
      return _activatedManagers.Contains(managerKey);
    }
  }
  
  public enum ManagerKey
  {
    GameManager,
    DataManager,
    SettingsManager,
    LoadingManager,
    SoundManager,
    PanelManager,
    ParticleManager,
  }
}