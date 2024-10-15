using System;
using Managers.Base;
using UnityEngine;

namespace Managers.Manager
{
  public class GameManager : ManagerBase
  {
    public static GameManager Instance { get; private set; }
    
    private GameState _gameState = GameState.None;
    public GameState CurrentGameState
    {
      get => _gameState;
      private set
      {
        _gameState = value;
        GameStateChanged?.Invoke();
      }
    }

    public Action GameStateChanged;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }
    
    public override void Initialize()
    {
      AddAction(ref Instance.GameStateChanged, OnGameStateChange);
    }

    private void OnGameStateChange()
    {
      if (ActivatedGameStates.Contains(Instance.CurrentGameState))
      {
        Activate();
      }
    }

    public void ChangeGameState(GameState gameState)
    {
      if (gameState == CurrentGameState) return;
      
      CurrentGameState = gameState;
      
      Debug.Log("<color=green>Game State Changed: " + CurrentGameState + "</color>");
    }
  }

  public enum GameState
  {
    None,
    LoadGame,
    MainMenu,
    LoadAssets,
    Game,
    TestScene,
  }

  public enum GameScenes
  {
    MainMenu,
  }
}