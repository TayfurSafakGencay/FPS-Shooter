using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Managers.Base;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserInterface.Panel;

namespace Managers.Manager
{
  public class LoadingManager : ManagerBase
  {
    public static LoadingManager Instance { get; private set; }
    
    private readonly List<Task> _loadingTasks = new();

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

    private LoadingPanel _loadingPanel;
    public async void StartLoadingScene()
    {
      PanelManager.Instance.ReleaseAllPanelsAsync();

      await PanelManager.Instance.CreatePanel(PanelKey.LoadingPanel);
      _loadingPanel = PanelManager.Instance.GetPanelHandle(PanelKey.LoadingPanel).Result.GetComponent<LoadingPanel>();
      
      StartCoroutine(LoadSceneWithAdditionalOperations(SceneName.GameScene));
    }

    public IEnumerator LoadSceneWithAdditionalOperations(SceneName sceneName)
    {
      LoadAssets();
      
      AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
      sceneLoadOperation.allowSceneActivation = false;
      float totalProgress = 0f;

      while (sceneLoadOperation.progress < 0.9f || _loadingTasks.Count > 0 || totalProgress < 0.99f)
      {
        float sceneProgress = Mathf.Clamp01(sceneLoadOperation.progress / 0.9f);

        float additionalProgress = _loadingTasks.Count == 0 ? 1f : 1f - (float)_loadingTasks.Count / _loadingTaskCount;

        totalProgress = sceneProgress * 0.7f + additionalProgress * 0.3f;
        _loadingPanel.UpdateLoadingPercentage(totalProgress);
        yield return null;
      }
      

      if (sceneLoadOperation.progress >= 0.9f && _loadingTasks.Count == 0)
      {
        StartCoroutine(GetProgressBarValue(sceneLoadOperation));
      }
    }

    private IEnumerator GetProgressBarValue(AsyncOperation sceneLoadOperation)
    {
      while (_loadingPanel.GetLoadingPercentage() < 0.99f) 
      {
        yield return new WaitForSeconds(2f);
      }
      
      sceneLoadOperation.allowSceneActivation = true;
      GameManager.Instance.ChangeGameState(GameState.Game);
    }

    private int _loadingTaskCount;
    public async void LoadAssets()
    {
      _loadingTaskCount = 0;
      
      AddAndRunTask(() => SoundManager.Instance.LoadAudioData(AudioDataAddressableKey.GameAudioData));
    
      await MonitorTasksAsync();
    }
    
    private void AddAndRunTask(Func<Task> taskFunc)
    {
      Task task = taskFunc();
      _loadingTasks.Add(task);
      _loadingTaskCount++;
    }
    
    private async Task MonitorTasksAsync()
    {
      while (_loadingTasks.Count > 0)
      {
        Task completedTask = await Task.WhenAny(_loadingTasks); 
        _loadingTasks.Remove(completedTask);  
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