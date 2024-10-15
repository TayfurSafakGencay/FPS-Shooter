﻿using System;
using Managers.Manager;
using Panels.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
  public class LoadingPanel : BasePanel
  {
    [SerializeField]
    private TextMeshProUGUI _loadingPercentageText;
    
    [SerializeField]
    private Image _loadingSlider;
    
    private float _targetPercentage;
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