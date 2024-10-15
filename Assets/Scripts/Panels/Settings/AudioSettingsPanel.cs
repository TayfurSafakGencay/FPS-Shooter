﻿using Managers.Manager;
using Panels.Base;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace Panels.Settings
{
  public class AudioSettingsPanel : BasePanel
  {
    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.SettingsPanelContent;
    }
    
    [Header("Master Audio")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private TextMeshProUGUI _masterValueText;
    
    public void OnMasterSliderValueChanged()
    {
      SoundManager.Instance.SetMasterVolume(_masterSlider.value);
      _masterValueText.text = $"{(int)(_masterSlider.value * 100)}%";
    }
    
    [Header("Music Audio")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private TextMeshProUGUI _musicValueText;
    
    public void OnMusicSliderValueChanged()
    {
      SoundManager.Instance.SetMusicVolume(_musicSlider.value);
      _musicValueText.text = $"{(int)(_musicSlider.value * 100)}%";
    }
    
    [Header("SFX Audio")]
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _sfxValueText;
    
    public void OnSfxSliderValueChanged()
    {
      SoundManager.Instance.SetSFXVolume(_sfxSlider.value);
      _sfxValueText.text = $"{(int)(_sfxSlider.value * 100)}%";
    }
    
    [Header("UI Audio")]
    [SerializeField] private Slider _uiSlider;
    [SerializeField] private TextMeshProUGUI _uiValueText;
    
    public void OnUiSliderValueChanged()
    {
      SoundManager.Instance.SetUIVolume(_uiSlider.value);
      _uiValueText.text = $"{(int)(_uiSlider.value * 100)}%";
    }
  }
}