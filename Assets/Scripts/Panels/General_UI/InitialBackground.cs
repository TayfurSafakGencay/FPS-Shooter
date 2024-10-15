using DG.Tweening;
using Managers.Manager.Settings;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Panels.General_UI
{
  [RequireComponent(typeof(Image))]
  public class InitialBackground : MonoBehaviour
  {
    private Image _backgroundImage;

    private void Awake()
    {
      _backgroundImage = GetComponent<Image>();
    }

    private void Start()
    {
      _backgroundImage.DOColor(new Color(0, 0, 0, 0.5f), 1.5f).OnComplete(() =>
        {
          _backgroundImage.DOColor(new Color(0, 0, 0, 0f), 0.5f).OnComplete(AnimationCompleted);
        });

      SettingsManager.Instance.DisableDeviceControls();
    }

    private void AnimationCompleted()
    {
      SettingsManager.Instance.EnableDeviceControls();

      Destroy(this);
    }
  }
}