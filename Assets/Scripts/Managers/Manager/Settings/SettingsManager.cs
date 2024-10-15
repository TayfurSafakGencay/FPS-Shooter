using Managers.Base;

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

    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChange);
    }

    private void OnGameStateChange()
    {
    }
  }
}