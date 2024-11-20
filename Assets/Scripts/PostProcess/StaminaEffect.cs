using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UserInterface.General;

namespace PostProcess
{
  public class StaminaEffect : MonoBehaviour
  {
    [SerializeField]
    private VolumeProfile _volumeProfile;
    
    private static Vignette _vignette;

    private void Awake()
    {
      if (_volumeProfile.TryGet<Vignette>(out Vignette vignette))
      {
        _vignette = vignette;
      }
      
      _vignette.intensity.value = 0;
    }

    public static void OnHealthChange(float currentHealth, float maxHealth)
    {
      float damage = maxHealth - currentHealth;
      float damagePercentage = damage / maxHealth;
      
      DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.Override(x), 
        damagePercentage, UserInterfaceTimes.DamageEffectTime);
    }

    public static void OnStaminaChanged(float stamina)
    {
     float vignetteValue = 0.375f * (1 - stamina / 100f);
     _vignette.intensity.value = vignetteValue;
    }
  }
}