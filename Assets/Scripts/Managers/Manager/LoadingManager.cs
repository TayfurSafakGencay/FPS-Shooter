using System.Collections;
using Managers.Base;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.Manager
{
  public class LoadingManager : ManagerBase
  {
    public static LoadingManager Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
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
    }

    public IEnumerator LoadSceneWithProgress(SceneName sceneName)
    {
      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName.ToString());
    
      while (!operation.isDone)
      {
        float progress = Mathf.Clamp01(operation.progress / 0.9f);
        Debug.Log("Yükleme ilerlemesi: " + progress * 100 + "%");
        
        yield return null;
      }

      if (operation.isDone)
      {
        Debug.Log("Yükleme ilerlemesi: " + 100 + "%");
      }
    }
  }
  
  public enum SceneName
  {
    MainMenu,
    GameScene,
    TestScene
  }
}