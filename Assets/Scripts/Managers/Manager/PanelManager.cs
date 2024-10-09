using System.Collections.Generic;
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

    private void OnGameStateChange()
    {
      if (ActivatedGameStates.Contains(GameManager.Instance.CurrentGameState))
      {
        Activate();
      }
      
      switch (GameManager.Instance.CurrentGameState)
      {
        case GameState.MainMenu:
          CreatePanel(PanelKey.MainMenuPanel);
          break;
      }
    }

    private async void CreatePanel(PanelKey panelKey)
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
  }

  public enum PanelKey
  {
    MainMenuPanel,  
  }
  
  public struct PanelLayer
  {
    public const int MainMenuPanel = 10;
  }
}