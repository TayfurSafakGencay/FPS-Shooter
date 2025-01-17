using System.Collections;
using System.Collections.Generic;
using Systems.Chase;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Enemy.Zombie
{
  public class ZombieSound : MonoBehaviour
  {
    public AudioSource AudioSource;

    public AudioClip ScreamSound;

    public List<AudioClip> GrumbleSounds;
    
    public List<AudioClip> AttackSounds;
    
    public List<AudioClip> DeathSounds;

    private Enemy _enemy;
    
    private bool _isScreaming;
    
    private const float _normalMaxDistance = 10;
    
    private const float _screamMaxDistance = 35;

    private void Awake()
    {
      _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
      StartCoroutine(PlayRandomGrumbleSound());
    }

    private IEnumerator PlayRandomGrumbleSound()
    {
      while (true)
      {
        yield return new WaitForSeconds(Random.Range(5f, 20f));

        if (_enemy.IsDead) break;
        if (_isScreaming) continue;
        if (_isAttacking) continue;

        AudioSource.maxDistance = _normalMaxDistance;
        AudioSource.volume = Random.Range(0.5f, 1f);
        int index = Random.Range(0, GrumbleSounds.Count);
        AudioSource.clip = GrumbleSounds[index];
        AudioSource.Play();
      }
    }

    public async void PlayScreamSound()
    {
      if (_enemy.IsDead) return;
      
      // float screamSoundLength = ScreamSound.length;
      // if (ChaseSystem.Screaming) return; 
      // ChaseSystem.Screamed(screamSoundLength);
      
      _isScreaming = true;

      AudioSource.maxDistance = _screamMaxDistance;
      AudioSource.volume = 1;
      AudioSource.clip = ScreamSound;
      AudioSource.Play();

      // await Utility.Delay(screamSoundLength);
      // _isScreaming = false;
    }

    private bool _isAttacking;
    public async void PlayAttackSound()
    {
      if (_enemy.IsDead) return;
      if (_isAttacking) return;

      AudioSource.maxDistance = _normalMaxDistance;
      AudioSource.volume = 0.75f;
      int index = Random.Range(0, AttackSounds.Count);
      AudioSource.clip = AttackSounds[index];
      AudioSource.Play();
      
      _isAttacking = true;
      await Utility.Delay(AttackSounds[index].length + 0.5f);
      _isAttacking = false;
    }
    
    public void PlayDeadSound()
    {
      AudioSource.maxDistance = _normalMaxDistance;
      AudioSource.volume = Random.Range(0.75f, 1f);
      int index = Random.Range(0, DeathSounds.Count);
      AudioSource.clip = DeathSounds[index];
      AudioSource.Play();
    }

    public void Respawn()
    {
      _isScreaming = false;
      _isAttacking = false;
      
      StartCoroutine(PlayRandomGrumbleSound());
    }
  }
}