using Managers.Manager;
using UserInterface.Panel.Base;

namespace UserInterface.Panel
{
  public class DeathPanel : BasePanel
  {
    public void OpenMainMenu()
    {
      LoadingManager.Instance.StartMainMenu();
      
      gameObject.SetActive(false);
    }

    public void RestartGame()
    {
      LoadingManager.Instance.Restart();
      
      gameObject.SetActive(false);
    }

    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.DeathPanel;
    }
  }
}