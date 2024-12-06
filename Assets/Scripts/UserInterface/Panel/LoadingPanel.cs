using Managers.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.Panel.Base;

namespace UserInterface.Panel
{
  public class LoadingPanel : BasePanel
  {
    [SerializeField]
    private TextMeshProUGUI _loadingPercentageText;
    
    [SerializeField]
    private Image _loadingSlider;
    
    private float _targetPercentage;

    public override void Awake()
    {
      base.Awake();
      
      SoundManager.Instance.SetVolumeFromPlayerPrefs();
    }

    protected override void ChangePanelLayer()
    {
      SortingGroup.sortingOrder = PanelLayer.LoadingPanel;
    }
    
    public void UpdateLoadingPercentage(float percentage)
    {
      _targetPercentage = percentage;
    }
    
    public float GetLoadingPercentage()
    {
      return _loadingSlider.fillAmount;
    }

    private void FixedUpdate()
    {
      _loadingSlider.fillAmount = Mathf.Lerp(_loadingSlider.fillAmount, _targetPercentage, Time.deltaTime);
      _loadingPercentageText.text = Mathf.RoundToInt(_loadingSlider.fillAmount * 100) + "%";
    }
  }
}