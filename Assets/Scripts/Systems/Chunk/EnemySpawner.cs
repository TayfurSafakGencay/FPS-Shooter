using System;
using Systems.EndGame;
using UnityEngine;
using ZombiePool;

namespace Systems.Chunk
{
  public class EnemySpawner : MonoBehaviour
  {
    private MeshRenderer _meshRenderer;
    
    private Collider _collider;
    
    private bool _active = true;

    private void Awake()
    {
      gameObject.name = "EnemySpawner: " + transform.GetSiblingIndex();
      
      _meshRenderer = GetComponent<MeshRenderer>();
      _meshRenderer.enabled = false;
      
      _collider = GetComponent<Collider>();
      _collider.enabled = true;
    }

    private void Start()
    {
      EndGameSystem.Instance.AddZombieSpawner();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("PlayerSpawnerTrigger"))
      {
        SpawnEnemy();
      }
    }

    public void SpawnEnemy()
    {
      if (!_active) return;
      _active = false;
      
      EndGameSystem.Instance.AddZombieCount();

      _collider.enabled = false;
      enabled = false;

      Enemy.Zombie.Enemy enemy = ZombiePoolManager.Instance.GetFromPool();
      enemy.gameObject.transform.position = transform.position;
      enemy.gameObject.SetActive(true);
      enemy.Respawn();
      
      gameObject.SetActive(false);
    }
  }
}