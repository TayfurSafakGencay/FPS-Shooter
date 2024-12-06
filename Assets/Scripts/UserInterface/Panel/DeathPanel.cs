using Managers.Manager;
using UserInterface.Panel.Base;

namespace UserInterface.Panel
{
  public class DeathPanel : BasePanel
  {
    public void OpenMainMenu()
    {
      LoadingManager.Instance.StartMainMenu();
    }

    public void RestartGame()
    {
      LoadingManager.Instance.Restart();
    }

    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.DeathPanel;
    }
  }
}