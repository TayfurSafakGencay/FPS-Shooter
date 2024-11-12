using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
  public class PlayerStep : MonoBehaviour
  {
    [SerializeField]
    private float _walkingStepInterval = 0.5f;

    [SerializeField]
    private float _sprintingStepInterval = 0.3f;

    [SerializeField]
    private List<AudioClip> _stepSounds;

    [SerializeField]
    private AudioSource _audioSource;

    private FirstPersonController _firstPersonController;
    
    private float _stepTimer;

    private void Awake()
    {
      _firstPersonController = GetComponent<FirstPersonController>();
    }

    private void Update()
    {
      if (_stepTimer > 0)
      {
        _stepTimer -= Time.deltaTime;
      }
      else if (_firstPersonController.GetIsGrounded() && _firstPersonController.GetIsMoving())
      {
        if (_firstPersonController.GetIsRunning())
        {
          PlayStepSound();
          _stepTimer = _sprintingStepInterval;
        }
        else
        {
          PlayStepSound();
          _stepTimer = _walkingStepInterval;
        }
      }
    }

    private void PlayStepSound()
    {
      _audioSource.clip = _stepSounds[Random.Range(0, _stepSounds.Count)];
      _audioSource.Play();
    }
  }
}