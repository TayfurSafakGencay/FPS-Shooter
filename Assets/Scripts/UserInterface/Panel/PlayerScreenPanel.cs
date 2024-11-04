using Actor;
using TMPro;
using UnityEngine;
using UserInterface.General;

namespace UserInterface.Panel
{
  public class PlayerScreenPanel : MonoBehaviour
  {
    [HideInInspector]
    public Player Player;

    [SerializeField]
    private TextMeshProUGUI _ammoText;

    [SerializeField]
    private CrosshairController _crosshairController;

    private void Awake()
    {
      Player = FindObjectOfType<Player>();
    }

    private void Update()
    {
      _ammoText.SetText($"{Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentClipAmmo}" +
                        $" " + "/" + Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentAmmo);
    }
    
    
  }
}