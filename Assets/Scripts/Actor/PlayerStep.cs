using System;
using System.Collections.Generic;
using Base.Interface;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Actor
{
  public class PlayerStep : MonoBehaviour
  {
    [SerializeField]
    private float _walkingStepInterval = 0.5f;

    [SerializeField]
    private float _sprintingStepInterval = 0.3f;
    
    [SerializeField]
    private LayerMask _groundLayer;

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
      if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2.5f, _groundLayer))
      {
        if (hit.collider.TryGetComponent(out Walkable walkable))
        {
          switch (walkable.WalkableType)
          {
            case WalkableType.Water:
              WaterSound();
              break;
            case WalkableType.Terrain:
              TerrainSound();
              break;
            case WalkableType.Wood:
              WoodSound();
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
    }

    [Header("Terrain Sounds")]
    [SerializeField]
    private List<AudioClip> _terrainStepSounds;
    private void TerrainSound()
    {
      _audioSource.clip = _terrainStepSounds[Random.Range(0, _terrainStepSounds.Count)];
      _audioSource.Play();
    }
    
    [Header("Water Sounds")]
    [SerializeField]
    private List<AudioClip> _waterStepSounds;
    private void WaterSound()
    {
      _audioSource.clip = _waterStepSounds[Random.Range(0, _waterStepSounds.Count)];
      _audioSource.Play();
    }
    
    [Header("Wood Sounds")]
    [SerializeField]
    private List<AudioClip> _woodStepSounds;
    private void WoodSound()
    {
      _audioSource.clip = _woodStepSounds[Random.Range(0, _woodStepSounds.Count)];
      _audioSource.Play();
    }
  }
}