using Audio;
using Managers.Manager;
using Panels.Base;

namespace Panels
{
  public class MainMenuPanel : BasePanel
  {
    private void Start()
    {
      SoundManager.Instance.PlayMusic(SoundKey.MainMenuMusic);
    }

    public void StartGame()
    {
      LoadingManager.Instance.StartLoadingScene();
    }
    
    public async void Options()
    {
      await PanelManager.Instance.CreatePanel(PanelKey.SettingsPanel);
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
      // Editörde oyunu durdurur
      UnityEditor.EditorApplication.isPlaying = false;
#else
        // Oyunu tamamen kapatır
        Application.Quit();
#endif
    }

    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.MainMenuPanel;
    }
  }
}