using System;
using UnityEngine;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Audio Config", menuName = "Tools/Guns/Audio Config", order = 0)]
  public class AudioConfig : ScriptableObject
  {
    public SoundVo FireClip;

    public SoundVo LastBulletsClip;

    public SoundVo EmptyMagClip;

    public SoundVo[] ReloadClips;
    
    public void PlayShootingClip(AudioSource audioSource, int bulletCount)
    {
      PlaySound(audioSource, bulletCount > 6 ? FireClip : LastBulletsClip);
    }
    
    public void PlayOutOfAmmoClip(AudioSource audioSource)
    {
      PlaySound(audioSource, EmptyMagClip);   
    }
    
    public void PlayReloadClip(AudioSource audioSource, int section)
    {
      if (ReloadClips != null)
      {
        PlaySound(audioSource, ReloadClips[section]);
      }
    }

    private void PlaySound(AudioSource audioSource, SoundVo soundVo)
    {
      audioSource.PlayOneShot(soundVo.AudioClip, soundVo.Volume);
    }
    
    [Serializable]
    public record SoundVo
    {
      public AudioClip AudioClip;
      
      [Range(0, 1)]
      public float Volume = 0.3f;
    }
  }
}