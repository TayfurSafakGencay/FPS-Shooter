using UnityEngine;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Audio Config", menuName = "Tools/Guns/Audio Config", order = 0)]
  public class AudioConfig : ScriptableObject
  {
    [Range(0, 1f)]
    public float Volume = 1f;

    public AudioClip[] FireClips;

    public AudioClip LastBulletsClip;

    public AudioClip EmptyClip;

    public AudioClip ReloadClip;
    
    public void PlayShootingClip(AudioSource audioSource, int bulletCount)
    {
      if (bulletCount > 15)
      {
        audioSource.PlayOneShot(FireClips[0], Volume);
      }
      else if (bulletCount > 6)
      {
        audioSource.PlayOneShot(FireClips[1], Volume);
      }
      else if (bulletCount <= 5)
      {
        audioSource.PlayOneShot(LastBulletsClip, Volume);
      }
    }
    
    public void PlayOutOfAmmoClip(AudioSource audioSource)
    {
      if (EmptyClip != null)
      {
        audioSource.PlayOneShot(EmptyClip, Volume);
      }
    }
    
    public void PlayReloadClip(AudioSource audioSource)
    {
      if (ReloadClip != null)
      {
        audioSource.PlayOneShot(ReloadClip, Volume);
      }
    }
  }
}