using Actor;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LootSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.General;
using Utilities;

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
    
    [SerializeField]
    private Image _staminaSlider;
    
    [SerializeField]
    private Image _bloodScreen;

    private void Awake()
    {
      Player = FindObjectOfType<Player>();
      
      _lootBackground.SetActive(false);
      
      _ammoText.SetText("");
      _crosshairController.gameObject.SetActive(true);
      _gunImage.enabled = false;
      _bloodScreen.enabled = false;
    }

    private void Start()
    {
      Player.SetPlayerScreenPanel(this);
    }

    private void Update()
    {
      _healthSlider.fillAmount = Player.GetPlayerHealth().GetHealthPercentage();
      _staminaSlider.fillAmount = Player.GetPlayerStamina().GetStaminaPercentage();
      _pillCountText.text = "x" + Player.GetInventory().GetConsumableCount(LootKey.Pill);

      if(!Player.GetPlayerGunSelector().HasGun) return;
      
      _ammoText.SetText($"<size=34><color=#00FF00>{Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentClipAmmo}</color></size>\n" +
                        $"<size=17><color=#FFFFFF>{Player.GetPlayerGunSelector().ActiveGun.AmmoConfig.CurrentAmmo}</color></size>");
      
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

    private TweenerCore<Color, Color, ColorOptions> _bloodFadeAnimation;
    public async void OnGetPlayerDamage(float health)
    {
      if (_bloodFadeAnimation != null && _bloodFadeAnimation.IsActive())
        _bloodFadeAnimation.Kill();
      
      _bloodScreen.enabled = true;
      if (health <= 20)
      {
        OnHealthCritical();
      }
      else
      {
        _bloodScreen.color = new Color(1, 1, 1, 1 - health / 100);

        await Utility.Delay(0.5f);
          
        _bloodFadeAnimation = _bloodScreen.DOColor(new Color(1,1,1,0), 0.5f).OnComplete(() => _bloodScreen.enabled = false);
      }
    }

    private TweenerCore<Vector3, Vector3, VectorOptions> _criticalHealthAnimation;
    public void OnHealthCritical()
    {
      if (_criticalHealthAnimation != null && _criticalHealthAnimation.IsActive() && _criticalHealthAnimation.IsPlaying())
        return;

      _bloodScreen.enabled = true;
      _bloodScreen.color = new Color(1, 1, 1, 1);
      
      _criticalHealthAnimation = _bloodScreen.transform.DOScale(1.05f, 0.25f)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnHealthRecovered(float health)
    {
      if (_criticalHealthAnimation == null || !_criticalHealthAnimation.IsActive()) return;
      if (!(health > 20)) return;
      
      _criticalHealthAnimation.Kill();
      _bloodScreen.enabled = false;
    }
    
    public void OnDeath()
    {
      _bloodScreen.enabled = false;
      
      gameObject.SetActive(false);
    }
  }
}