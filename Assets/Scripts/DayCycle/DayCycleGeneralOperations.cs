using System;
using System.Threading.Tasks;
using Audio;
using DG.Tweening;
using Managers.Manager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Utilities;

namespace DayCycle
{
  public class DayCycleGeneralOperations : MonoBehaviour
  {
    [SerializeField]
    private Material _morningSkybox;
    
    [SerializeField]
    private Material _noonSkybox;
    
    [SerializeField]
    private Material _eveningSkybox;
    
    [SerializeField]
    private Material _nightSkybox;

    private Color _skyboxInitialTintColor;

    [Header("Post Process")]
    [SerializeField]
    private Volume _volume;
    
    [SerializeField]
    private VolumeProfile _morningPostProcess;
    
    private Bloom _morningBloom;
    
    private ColorAdjustments _morningColorAdjustments;
    
    [SerializeField]
    private VolumeProfile _nightPostProcess;
    
    private Bloom _nightBloom;
    
    private ColorAdjustments _nightColorAdjustments;
    
    private float _skyboxTransitionDuration;
    
    private float _waitingBeforeSkyboxChange;

    private void Awake()
    {
      DayCycleSystem.OnDayTimeChanged += OnDayTimeChanged;
      
      _skyboxInitialTintColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
      _morningSkybox.SetColor("_Tint", _skyboxInitialTintColor);
      _noonSkybox.SetColor("_Tint", _skyboxInitialTintColor);
      _eveningSkybox.SetColor("_Tint", _skyboxInitialTintColor);
      _nightSkybox.SetColor("_Tint", _skyboxInitialTintColor);
      
      _skyboxTransitionDuration = DayCycleSystem.DayTimeChangingTime * 0.1f;
      _waitingBeforeSkyboxChange = DayCycleSystem.DayTimeChangingTime * 0.9f;
      
      _morningPostProcess.TryGet(out _morningBloom);
      _morningPostProcess.TryGet(out _morningColorAdjustments);
      
      _nightPostProcess.TryGet(out _nightBloom);
      _nightPostProcess.TryGet(out _nightColorAdjustments);
    }

    private void Start()
    {
      Morning();
      StartSkyboxRotation();
    }

    private void OnDayTimeChanged(DayTime dayTime)
    {
      switch (dayTime)
      {
        case DayTime.Morning:
          Morning();
          break;
        case DayTime.Noon:
          Noon();
          break;
        case DayTime.Evening:
          Evening();
          break;
        case DayTime.Night:
          Night();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(dayTime), dayTime, null);
      }
    }
    
    private readonly Color _noonSkyboxColor = new(0.5f, 0.5f, 0.5f, 0.5f);
    private async void Morning()
    {
      AnimatePostProcessing(_morningBloom, _morningColorAdjustments, 1, 0.5f);
      _morningSkybox.SetColor("_Tint", new Color(0.85f, 0.85f, 0.85f, 0.5f));

      SoundManager.Instance.PlayAmbienceSound(SoundKey.MorningAmbiance, 0.35f, 1, true);
      DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0, 10f);
      _volume.profile = _morningPostProcess;

      await Utility.Delay(_waitingBeforeSkyboxChange);
      
      await SmoothTransition(_noonSkyboxColor, _morningSkybox, _noonSkybox);
    }
    

    private async void Noon()
    {
      AnimatePostProcessing(_morningBloom, _morningColorAdjustments, 0.5f, 0.3f);

      await Utility.Delay(_waitingBeforeSkyboxChange);
      
      await SmoothTransition(Color.blue, _noonSkybox, _eveningSkybox);
    }
    
    
    private void Evening()
    {
      SoundManager.Instance.PlayAmbienceSound(SoundKey.NightAmbiance, 1f, 1, true);
      DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.175f, 20f).SetEase(Ease.InQuad);
      RenderSettings.fogColor = Color.gray;
      _volume.profile = _nightPostProcess;
    }
    
    private async void Night()
    {
      DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, Color.black, 30).SetEase(Ease.Linear);   
      DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.05f, 20f);
      
      await Utility.Delay(_waitingBeforeSkyboxChange);
      await SmoothTransition(new Color(0.9f, 0.9f, 0.9f, 0.5f), _nightSkybox, _morningSkybox);
    }
    
    private async Task SmoothTransition(Color targetColor, Material oldSkybox, Material newSkyBox)
    {
      Tween skyboxColorChanging = oldSkybox.DOColor(targetColor, "_Tint", _skyboxTransitionDuration).OnComplete(() =>
      {
        RenderSettings.skybox = newSkyBox;
        StartSkyboxRotation();
        oldSkybox.SetColor("_Tint", _skyboxInitialTintColor);
      });
      
      await skyboxColorChanging.AsyncWaitForCompletion();
    }
    
    private void ChangeSkyboxExposure(float targetExposure, Material skyboxMaterial)
    {
      DOTween.To(() =>
        skyboxMaterial.GetFloat("_Exposure"), x =>
        skyboxMaterial.SetFloat("_Exposure", x), targetExposure, _skyboxTransitionDuration).OnComplete(() =>
      {
        skyboxMaterial.SetFloat("_Exposure", 1f);
      });
    }

    private void AnimatePostProcessing(Bloom bloom, ColorAdjustments colorAdjustments, float bloomIntensity, float postExposure)
    {
      DOTween.To(() => bloom.intensity.value, x => bloom.intensity.value = x,
        bloomIntensity, _waitingBeforeSkyboxChange).SetEase(Ease.InOutSine);

      DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x,
        postExposure, _waitingBeforeSkyboxChange).SetEase(Ease.InOutSine);
    }

    private void StartSkyboxRotation()
    {
      DOTween.To(() => RenderSettings.skybox.GetFloat("_Rotation"), 
          x => RenderSettings.skybox.SetFloat("_Rotation", x), 
          360f, DayCycleSystem.DayTimeChangingTime)
        .SetEase(Ease.Linear)
        .SetOptions(true);
    }
  }
}