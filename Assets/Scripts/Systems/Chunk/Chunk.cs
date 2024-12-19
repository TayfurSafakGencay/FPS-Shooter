using System;
using System.Collections.Generic;
using Systems.EndGame;
using UnityEngine;
using Utilities;

namespace Systems.Chunk
{
  public class Chunk : MonoBehaviour
  {
    private List<EnemySpawner> _enemySpawners;
    
    private bool _active;
    private void Awake()
    {
      _enemySpawners = new List<EnemySpawner>();
      foreach (EnemySpawner enemySpawner in GetComponentsInChildren<EnemySpawner>())
      {
        _enemySpawners.Add(enemySpawner);
      }
    }

    private void Start()
    {
      Init();
    }

    private async void Init()
    {
      EndGameSystem.Instance.AddChunkCount();
      _active = false;
      await Utility.Delay(1f);
      _active = true;
    }
    private void OnTriggerEnter(Collider other)
    {
      if (!_active) return;
      if (!other.CompareTag("Player")) return;

      ActivateSpawners();
    }

    private void ActivateSpawners()
    {
      for (int i = 0; i < _enemySpawners.Count; i++)
      {
        _enemySpawners[i].SpawnEnemy();
      }
      
      gameObject.GetComponent<Chunk>().enabled = false;
        EndGameSystem.Instance.ChunkActivated();
    }
  }
}