using Managers.Manager;
using TMPro;
using UnityEngine;

namespace Panels.Settings
{
  public class SettingsPanel : BasePanel
  {
    [SerializeField] 
    private TextMeshProUGUI _titleText;
    
    private GameObject _openedPanel;
    
    private GameObject _openedHighlight;
    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.SettingsPanel;
    }

    public override void Awake()
    {
      base.Awake();
      
      InitialSettings();
    }

    private void InitialSettings()
    {
      _openedPanel = _audioSettingsPanel;
      _openedHighlight = _audioButtonHighlight;
      
      OpenAudioSettings();

      _controlsSettingsPanel.SetActive(false);
      _controlsButtonHighlight.SetActive(false);
    }
    
    private void OpenSettingsContentPanel(ref GameObject panel, ref GameObject highlight, string title)
    {
      _openedPanel.gameObject.SetActive(false);
      _openedHighlight.gameObject.SetActive(false);

      _openedPanel = panel;
      _openedHighlight = highlight;
      
      _openedPanel.gameObject.SetActive(true);
      _openedHighlight.gameObject.SetActive(true);
      _titleText.text = title.ToUpper(new System.Globalization.CultureInfo("en-US"));
    }

    [Header("Audio Settings")]
    [SerializeField] private GameObject _audioSettingsPanel;

    [SerializeField] private GameObject _audioButtonHighlight;
    
    private const string _audioTitle = "Audio";
    
    public void OpenAudioSettings()
    {
      OpenSettingsContentPanel(ref _audioSettingsPanel, ref _audioButtonHighlight, _audioTitle);
    }
    
    
    [Header("Controls Settings")]
    [SerializeField] private GameObject _controlsSettingsPanel;
    
    [SerializeField] private GameObject _controlsButtonHighlight;
    
    private const string _controlsTitle = "Controls";
    
    public void OpenControlsSettings()
    {
      OpenSettingsContentPanel(ref _controlsSettingsPanel, ref _controlsButtonHighlight, _controlsTitle);
    }
  }
}