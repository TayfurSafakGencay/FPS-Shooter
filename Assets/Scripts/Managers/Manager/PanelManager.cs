﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Managers.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UserInterface.General;
using Utilities;

namespace Managers.Manager
{
  public class PanelManager : ManagerBase
  {
    public static PanelManager Instance { get; private set; }
    
    private readonly Dictionary<PanelKey, AsyncOperationHandle<GameObject>> _panelHandles = new();
    
    private Transform _panelContainer;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }
    
    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChange);
    }
    
    public void ChangePanelContainer()
    {
      ReleaseAllPanelsAsync();
      
      _panelContainer = Root.Instance.Canvas.transform;
    }

    private async void OnGameStateChange()
    {
      if (ActivatedGameStates.Contains(GameManager.Instance.CurrentGameState))
      {
        Activate();
      }
      
      switch (GameManager.Instance.CurrentGameState)
      {
        case GameState.MainMenu:
          ChangePanelContainer();
          await CreatePanel(PanelKey.MainMenuPanel);
          break;
        case GameState.Game:
          await Utility.Delay(UserInterfaceTimes.InitialVignetteEffectTime);
          ChangePanelContainer();
          await CreatePanel(PanelKey.PlayerScreenPanel);
          break;
        case GameState.Death:
          await CreatePanel(PanelKey.DeathPanel);
          break;
      }
    }

    public AsyncOperationHandle<GameObject> GetPanelHandle(PanelKey panelKey)
    {
      return _panelHandles[panelKey];
    }

    public async Task CreatePanel(PanelKey panelKey)
    {
      if (!_panelHandles.ContainsKey(panelKey))
      {
        AsyncOperationHandle<GameObject> panelHandle = Addressables.InstantiateAsync(panelKey.ToString(), _panelContainer);
        await panelHandle.Task;

        if (panelHandle.Status == AsyncOperationStatus.Succeeded) 
        {
          _panelHandles.Add(panelKey, panelHandle);
        }
        else
        {
          Debug.LogWarning("Panel could not created!");
        }
      }
      else
      {
        Debug.LogWarning($"{panelKey} already in the list!");
      }
    }
    
    public void RemovePanel(PanelKey panelKey)
    {
      if (_panelHandles.ContainsKey(panelKey))
      {
        Addressables.ReleaseInstance(_panelHandles[panelKey]);
        _panelHandles.Remove(panelKey);
      }
      else
      {
        Debug.LogWarning($"{panelKey} not found in the list!");
      }
    }
    
    public void ReleaseAllPanelsAsync()
    {
      foreach (KeyValuePair<PanelKey, AsyncOperationHandle<GameObject>> panelHandle in _panelHandles)
      {
        if (panelHandle.Value.IsValid())
          Addressables.ReleaseInstance(panelHandle.Value);
      }

      _panelHandles.Clear();     
    }
    
    public bool IsPanelActive(PanelKey panelKey)
    {
      return _panelHandles.ContainsKey(panelKey);
    }
  }

  public enum PanelKey
  {
    MainMenuPanel,  
    SettingsPanel,
    LoadingPanel,
    PlayerScreenPanel,
    DeathPanel,
  }
  
  public struct PanelLayer
  {
    public const int MainMenuPanel = 10;
    public const int DeathPanel = 15;
    public const int SettingsPanel = 20;
    public const int SettingsPanelContent = 30;
    public const int LoadingPanel = 1000;
  }
}