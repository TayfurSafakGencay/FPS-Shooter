﻿using DG.Tweening;
using Managers.Manager.Settings;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Panels.General_UI
{
  public class InitialBackground : MonoBehaviour
  {
    [SerializeField]
    private VolumeProfile _volumeProfile;
    
    private Vignette _vignette;
    
    private void Awake()
    {
      if(_volumeProfile.TryGet<Vignette>(out Vignette vignette))
      {
        _vignette = vignette;
      }
      
      _vignette.intensity.value = 1;
    }

    private void Start()
    {
      SettingsManager.Instance.DisableDeviceControls();

        DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.Override(x), 
          0, 2f).OnComplete(AnimationCompleted);
    }

    private void AnimationCompleted()
    {
      SettingsManager.Instance.EnableDeviceControls();

      Destroy(this);
    }
  }
}