using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UserInterface.General;
using Utilities;

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

    public static async void OnStaminaChanged()
    {
      DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.Override(x), 
        0.2f, UserInterfaceTimes.BreathEffectTime);

      await Utility.Delay(4f);
      
      DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.Override(x), 
        0f, UserInterfaceTimes.BreathEffectTime * 1.5f);
    }
  }
}