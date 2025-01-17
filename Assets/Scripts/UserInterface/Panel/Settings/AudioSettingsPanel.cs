using Managers.Manager;
using Managers.Manager.Settings;
using TMPro;
using UnityEngine;
using UserInterface.Panel.Base;
using Slider = UnityEngine.UI.Slider;

namespace UserInterface.Panel.Settings
{
  public class AudioSettingsPanel : BasePanel
  {
    private void OnEnable()
    {
      _masterSlider.value = SettingsManager.Instance.MasterVolume;
      _musicSlider.value = SettingsManager.Instance.MusicVolume;
      _sfxSlider.value = SettingsManager.Instance.SFXVolume;
      _uiSlider.value = SettingsManager.Instance.UIVolume;
      _ambienceSlider.value = SettingsManager.Instance.AmbienceVolume;
      
      _masterValueText.text = $"{(int)(_masterSlider.value * 100)}%";
      _musicValueText.text = $"{(int)(_musicSlider.value * 100)}%";
      _sfxValueText.text = $"{(int)(_sfxSlider.value * 100)}%";
      _uiValueText.text = $"{(int)(_uiSlider.value * 100)}%";
      _ambienceValueText.text = $"{(int)(_ambienceSlider.value * 100)}%";
    }

    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.SettingsPanelContent;
    }
    
    [Header("Master Audio")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private TextMeshProUGUI _masterValueText;
    
    public void OnMasterSliderValueChanged()
    {
      SettingsManager.Instance.SetMasterVolume(_masterSlider.value);
      _masterValueText.text = $"{(int)(_masterSlider.value * 100)}%";
    }
    
    [Header("Music Audio")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private TextMeshProUGUI _musicValueText;
    
    public void OnMusicSliderValueChanged()
    {
      SettingsManager.Instance.SetMusicVolume(_musicSlider.value);
      _musicValueText.text = $"{(int)(_musicSlider.value * 100)}%";
    }
    
    [Header("SFX Audio")]
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _sfxValueText;
    
    public void OnSfxSliderValueChanged()
    {
      SettingsManager.Instance.SetSFXVolume(_sfxSlider.value);
      _sfxValueText.text = $"{(int)(_sfxSlider.value * 100)}%";
    }
    
    [Header("UI Audio")]
    [SerializeField] private Slider _uiSlider;
    [SerializeField] private TextMeshProUGUI _uiValueText;
    
    public void OnUiSliderValueChanged()
    {
      SettingsManager.Instance.SetUIVolume(_uiSlider.value);
      _uiValueText.text = $"{(int)(_uiSlider.value * 100)}%";
    }
    
    [Header("Ambience Audio")]
    [SerializeField] private Slider _ambienceSlider;
    [SerializeField] private TextMeshProUGUI _ambienceValueText;

    public void OnAmbienceSliderValueChanged()
    {
      SettingsManager.Instance.SetAmbienceVolume(_ambienceSlider.value);
      _ambienceValueText.text = $"{(int)(_ambienceSlider.value * 100)}%";
    }
  }
}