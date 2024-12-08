using DG.Tweening;
using Managers.Manager.Settings;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UserInterface.General;

namespace PostProcess
{
  public class VignetteEffect : MonoBehaviour
  {
    [SerializeField]
    private VolumeProfile _volumeProfile;
    
    private static Vignette _vignette;
    
    private static ColorAdjustments _colorAdjustments;
    
    private void Awake()
    {
      if(_volumeProfile.TryGet<Vignette>(out Vignette vignette))
      {
        _vignette = vignette;
      }
      
      if(_volumeProfile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
      {
        _colorAdjustments = colorAdjustments;
      }
      
      _colorAdjustments.colorFilter.value = Color.black;
      _vignette.intensity.value = 1;
    }

    private void Start()
    {
      SettingsManager.Instance.DisableDeviceControls();

      InitialColorAdjustments();
        // DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.Override(x), 
        // 0.15f, UserInterfaceTimes.InitialVignetteEffectTime).OnComplete(AnimationCompleted);
    }
    
    public void InitialColorAdjustments()
    {
      DOTween.To(() => _colorAdjustments.colorFilter.value, x => _colorAdjustments.colorFilter.value = x,
        Color.white, UserInterfaceTimes.InitialVignetteEffectTime).OnComplete(AnimationCompleted);
    }

    public static void DeathVignette()
    {
      DOTween.To(() => _colorAdjustments.colorFilter.value, x => _colorAdjustments.colorFilter.value = x,
        Color.black, UserInterfaceTimes.DeathVignetteEffectTime * 2);
    }
    
    public static void GameCompleted()
    {
      DOTween.To(() => _colorAdjustments.colorFilter.value, x => _colorAdjustments.colorFilter.value = x,
        Color.black, UserInterfaceTimes.GameCompletedVignetteEffectTime);
    }

    private void AnimationCompleted()
    {
      SettingsManager.Instance.EnableDeviceControls();
    }
  }
}