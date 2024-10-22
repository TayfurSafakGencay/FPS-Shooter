using UnityEngine;

namespace Managers.Manager
{
  public class Root : MonoBehaviour
  {
    public static Root Instance { get; private set; }
    
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    public Canvas Canvas;
  }
}