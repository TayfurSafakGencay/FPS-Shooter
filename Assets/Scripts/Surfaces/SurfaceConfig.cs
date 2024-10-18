using System;
using Managers.Manager;
using UnityEngine;

namespace Surfaces
{
  [CreateAssetMenu(fileName = "Surface Config", menuName = "Tools/Surface/Surface Config", order = 0)]
  public class SurfaceConfig : ScriptableObject
  {
    public GameObject HitParticle;
    
    public AudioClip HitSound;
    
    public AudioSource AudioSource;

    [Range(0f, 1f)]
    public float Volume = 1f;

    private void Awake()
    {
      AudioSource.outputAudioMixerGroup = SoundManager.Instance.GetMixerGroupToAudioSourceForSFX();
    }

    public void PlayParticle(Vector3 position, Quaternion rotation)
    {
      GameObject hitParticle = Instantiate(HitParticle, position, rotation);
      AudioSetup(hitParticle);
    }
    
    private void AudioSetup(GameObject particle)
    {
      AudioSource audioSource = Instantiate(AudioSource, particle.transform);

      audioSource.clip = HitSound;
      audioSource.volume = Volume;
      audioSource.Play();
    }
  }
}