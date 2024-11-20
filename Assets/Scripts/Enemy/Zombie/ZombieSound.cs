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

    private Enemy _enemy;
    
    private bool _isScreaming;

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

        AudioSource.volume = Random.Range(0.5f, 1f);
        int index = Random.Range(0, GrumbleSounds.Count);
        AudioSource.PlayClipAtPoint(GrumbleSounds[index], transform.position);
      }
    }

    public async void PlayScreamSound()
    {
      float screamSoundLength = ScreamSound.length;
      if (ChaseSystem.Screaming) return; 
      ChaseSystem.Screamed(screamSoundLength);
      
      _isScreaming = true;

      AudioSource.volume = Random.Range(0.3f, 0.75f);
      AudioSource.clip = ScreamSound;
      AudioSource.Play();

      await Utility.Delay(screamSoundLength);
      _isScreaming = false;
    }
  }
}