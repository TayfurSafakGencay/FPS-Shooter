using Managers;
using Managers.Manager;
using UnityEngine;

namespace Audio
{
  public class PlaySound : MonoBehaviour
  {
    public SoundKey SoundKey;
    
    [Range(0, 1)]
    public float Volume = 1;

    public void Start()
    {
      SoundManager.Instance.PlaySound(SoundKey, Volume);
    }
  }
}