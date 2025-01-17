using System;
using Actor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Zombie
{
    public class ZombieRandomWalkerManager : MonoBehaviour
    {
        public static ZombieRandomWalkerManager Instance { get; private set; }

        [SerializeField]
        private Player _player;

        [Header("Terrains")]
        [SerializeField] private Terrain terrain1;
        [SerializeField] private Terrain terrain2;

        [Header("Zombie Settings")]
        private float updateInterval = 10f;

        private float _timeRemaining;

        public Action<Vector3> RandomlyWalk;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _timeRemaining = 3;
        }

        private void Update()
        {
            if (!ShouldUpdate()) return;
            
            UpdateZombieDestinations();
        }
        
        private bool ShouldUpdate()
        {
            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining <= 0)
            {
                _timeRemaining = updateInterval;
                return true;
            }

            return false;
        }

        private void UpdateZombieDestinations()
        {
            RandomlyWalk?.Invoke(_player.transform.position);
            // foreach (var zombie in zombies)
            // {
            //     if (!zombie || !zombie.gameObject.activeInHierarchy) continue;
            //
            //     if (distanceToPlayer > activeDistanceThreshold)
            //     {
            //         zombie.isStopped = true; // Stop far zombies
            //         continue;
            //     }
            //
            //     zombie.isStopped = false;
            //     zombie.SetDestination(GetRandomPointOnTerrain(zombie.transform.position));
            // }
        }

        public Vector3 GetRandomPointOnTerrain(Vector3 zombiePosition)
        {
            Terrain targetTerrain = zombiePosition.x < terrain1.transform.position.x + terrain1.terrainData.size.x ? terrain1 : terrain2;

            Vector3 terrainPosition = targetTerrain.transform.position;
            float terrainWidth = targetTerrain.terrainData.size.x;
            float terrainLength = targetTerrain.terrainData.size.z;

            float x = Random.Range(zombiePosition.x - 5, zombiePosition.x + 5);
            float z = Random.Range(zombiePosition.z - 5, zombiePosition.z + 5);
            float y = targetTerrain.SampleHeight(new Vector3(x, 0, z)) + terrainPosition.y;

            return new Vector3(x, y, z);
        }
    }
}
