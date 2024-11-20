using UnityEngine;
using Utilities;

namespace Actor
{
  public class BreathSound : MonoBehaviour
  {
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _breathSound;
    
    [SerializeField]
    private float _breathVolume = 0.1f;
    
    private bool _isPlaying;
    
    public async void PlayBreathSound()
    {
      if (_isPlaying) return;

      _isPlaying = true;
      _audioSource.clip = _breathSound;
      _audioSource.volume = _breathVolume;
      _audioSource.Play();

      await Utility.Delay(15f);
      _isPlaying = false;
    }
  }
}