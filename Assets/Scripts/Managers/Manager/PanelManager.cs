using System.Collections.Generic;
using System.Threading.Tasks;
using Managers.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

      _panelContainer = GameObject.FindWithTag("PanelCanvas").transform;
    }
    
    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChange);
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
          await CreatePanel(PanelKey.MainMenuPanel);
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
        Addressables.ReleaseInstance(panelHandle.Value);
      }

      _panelHandles.Clear();     
    }
  }

  public enum PanelKey
  {
    MainMenuPanel,  
    SettingsPanel,
    LoadingPanel,
  }
  
  public struct PanelLayer
  {
    public const int MainMenuPanel = 10;
    public const int SettingsPanel = 20;
    public const int SettingsPanelContent = 30;
    public const int LoadingPanel = 1000;
  }
}