using Audio;
using Managers;
using Managers.Manager;
using UnityEngine;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Audio Config", menuName = "Tools/Guns/Audio Config", order = 0)]
  public class AudioConfig : ScriptableObject
  {
    [Range(0, 1f)]
    public float Volume = 1f;

    public SoundKey FireClips;

    public SoundKey LastBulletsClip;

    public SoundKey EmptyMagClip;

    public SoundKey[] ReloadClips;

    private SoundManager _soundManager;
    private void Awake()
    {
      _soundManager = SoundManager.Instance;
    }

    public void PlayShootingClip(AudioSource audioSource, int bulletCount)
    {
      _soundManager.PlayOneShot(bulletCount > 6 ? FireClips : LastBulletsClip, audioSource, Volume);
    }
    
    public void PlayOutOfAmmoClip(AudioSource audioSource)
    {
      _soundManager.PlayOneShot(EmptyMagClip, audioSource, Volume);
    }
    
    public void PlayReloadClip(AudioSource audioSource, int section)
    {
      if (ReloadClips != null)
      {
        _soundManager.PlayOneShot(ReloadClips[section], audioSource, Volume);
      }
    }
  }
}