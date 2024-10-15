using Audio;
using Managers.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Panels.General_UI
{
  public class ButtonSounds : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
  {
    public void OnPointerEnter(PointerEventData eventData)
    {
      SoundManager.Instance.PlayUISound(SoundKey.ButtonHover, 0.15f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      SoundManager.Instance.PlayUISound(SoundKey.ButtonClick, 1, 2);
    }
  }
}