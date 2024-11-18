using Actor;
using LootSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    private Image _gunImage;

    [SerializeField]
    private TextMeshProUGUI _pillCountText;
    
    [SerializeField]
    private CrosshairController _crosshairController;
    
    [SerializeField]
    private TextMeshProUGUI _lootText;
    
    [SerializeField]
    private GameObject _lootBackground;
    
    [SerializeField]
    private Image _healthSlider;

    private void Awake()
    {
      Player = FindObjectOfType<Player>();
      
      _lootBackground.SetActive(false);
      
      _ammoText.SetText("");
      _gunImage.enabled = false;
    }

    private void Start()
    {
      Player.SetPlayerScreenPanel(this);
    }

    private void Update()
    {
      _healthSlider.fillAmount = Player.GetPlayerHealth().GetHealthPercentage();

      if(!Player.GetPlayerGunSelector().HasGun) return;
      
      _ammoText.SetText($"<size=30><color=#00FF00>{Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentClipAmmo}</color></size>\n" +
                        $"<size=15><color=#FFFFFF>{Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentAmmo}</color></size>");
      
      _pillCountText.text = "x" + Player.GetInventory().GetConsumableCount(LootKey.Pill);
    }
    
    public void OnGunSwitch()
    {
      _gunImage.sprite = Player.GetPlayerGunSelector().ActiveGun.GunIcon;
      _gunImage.enabled = true;
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