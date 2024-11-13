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
    
    [SerializeField]
    private TextMeshProUGUI _lootText;
    
    [SerializeField]
    private GameObject _lootBackground;

    private void Awake()
    {
      Player = FindObjectOfType<Player>();
      
      _lootBackground.SetActive(false);
    }

    private void Start()
    {
      Player.SetPlayerScreenPanel(this);
    }

    private void Update()
    {
      if(!Player.GetPlayerGunSelector().HasGun) return;
      
      _ammoText.SetText($"{Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentClipAmmo}" +
                        " " + "/" + Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentAmmo);
    }
    
    public void SetLootText(string text)
    {
      _lootText.SetText(text);
      _lootBackground.SetActive(true);
    }
    
    public void DisableLootText()
    {
      _lootBackground.SetActive(false);
    }
  }
}