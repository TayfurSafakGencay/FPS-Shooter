using Audio;
using Managers.Manager;
using Managers.Manager.Settings;
using UserInterface.Panel.Base;

namespace UserInterface.Panel
{
  public class MainMenuPanel : BasePanel
  {
    public override void Awake()
    {
      base.Awake();
      
      SoundManager.Instance.SetVolumeFromPlayerPrefs();
    }
    private void Start()
    {
      SettingsManager.Instance.EnableDeviceControls();

      SoundManager.Instance.PlayMusic(SoundKey.MainMenuMusic);
    }

    public void StartGame()
    {
      LoadingManager.Instance.StartLoadingScene();
      
      Destroy(gameObject);
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
        Application.Quit();
#endif
    }

    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.MainMenuPanel;
    }
  }
}