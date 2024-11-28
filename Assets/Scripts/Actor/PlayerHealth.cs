using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Random = UnityEngine.Random;

namespace Actor
{
  public class PlayerHealth : MonoBehaviour
  {
    private Player _player;
    public float MaxHealth { get; private set; } = 100;

    public float Health { get; private set; }

    public bool IsDead => Health <= 0;
    
    [FormerlySerializedAs("_audioSource")]
    [SerializeField]
    private AudioSource _hearthBeatAudioSource;

    [SerializeField]
    private AudioSource _damageAudioSource;
    
    [SerializeField]
    private List<AudioClip> _damageAudioClips;
    
    [SerializeField]
    private AudioClip _healAudioClip;
    
    private void Awake()
    {
      Health = MaxHealth;

      _player = GetComponent<Player>();
      
      _hearthBeatAudioSource.loop = true;
      _hearthBeatAudioSource.playOnAwake = false;
      _hearthBeatAudioSource.Stop();
    }

    public void TakeDamage(float damage)
    {
      if (IsDead) return;

      Health -= damage;

      if (Health <= 20) _hearthBeatAudioSource.Play(); 
      else _hearthBeatAudioSource.Stop();

      _player.GetPlayerScreenPanel().OnGetPlayerDamage(Health);
      
      _damageAudioSource.clip = _damageAudioClips[Random.Range(0, _damageAudioClips.Count)];
      _damageAudioSource.Play();

      if (Health <= 0)
      {
        Die();
      }
    }

    private const float _healthRegenRate = 0.5f;
    public async void HealingOverTime(bool sound = true)
    {
      if (Health >= MaxHealth) return;
      
      Heal(10, sound);
      await Utility.Delay(_healthRegenRate);
      HealingOverTime(false);
    }
    
    public void Heal(int healAmount, bool sound = true)
    {
      if (IsDead) return;

      Health += healAmount;
      
      if (Health <= 20) _hearthBeatAudioSource.Play(); 
      else _hearthBeatAudioSource.Stop();
      
      _player.GetPlayerScreenPanel().OnHealthRecovered(Health);

      if (sound)
      {
        _damageAudioSource.clip = _healAudioClip;
        _damageAudioSource.Play();
      }

      if (Health > MaxHealth)
      {
        Health = MaxHealth;
      }
    }

    public void Die()
    {
      Component[] components = gameObject.GetComponents<Component>();

      foreach (Component component in components)
      {
        switch (component)
        {
          case Transform:
            continue;
          case MonoBehaviour monoBehaviour:
            monoBehaviour.enabled = false;
            break;
        }
      }
    }

    public float GetHealthPercentage()
    {
      return Health / MaxHealth;
    }

    public bool IsFullHealth()
    {
      return Math.Abs(Health - MaxHealth) < 0.5f;
    }
  }
}