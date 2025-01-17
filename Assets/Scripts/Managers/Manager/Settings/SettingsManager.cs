using Managers.Base;
using UnityEngine.InputSystem;

namespace Managers.Manager.Settings
{
  public partial class SettingsManager : ManagerBase
  {
    public static SettingsManager Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Update()
    {
      if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;
      if (GameManager.Instance.CurrentGameState == GameState.Game)
      {
        OpenSettingsPanel();
      }
    }

    private async void OpenSettingsPanel()
    {
      if (PanelManager.Instance.IsPanelActive(PanelKey.SettingsPanel)) return;
      
      await PanelManager.Instance.CreatePanel(PanelKey.SettingsPanel);
    }

    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChange);
    }

    private void OnGameStateChange()
    {
    }
  }
}