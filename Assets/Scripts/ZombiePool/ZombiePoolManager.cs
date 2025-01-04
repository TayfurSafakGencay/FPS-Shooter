using System.Collections.Generic;
using UnityEngine;

namespace ZombiePool
{
    public class ZombiePoolManager : MonoBehaviour
    {
        public static ZombiePoolManager Instance { get; private set; }

        private readonly Queue<Enemy.Zombie.Enemy> _pool = new();

        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private Enemy.Zombie.Enemy _enemyPrefab;

        private const int initialPoolSize = 3;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewObject();
            }
        }

        private Enemy.Zombie.Enemy CreateNewObject()
        {
            Enemy.Zombie.Enemy enemy = Instantiate(_enemyPrefab, _parent);
            enemy.gameObject.SetActive(false);
            _pool.Enqueue(enemy);
            return enemy;
        }

        public Enemy.Zombie.Enemy GetFromPool()
        {
            Enemy.Zombie.Enemy obj;

            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                CreateNewObject();
                obj = _pool.Dequeue();
            }

            return obj;
        }

        public void ReturnToPool(Enemy.Zombie.Enemy obj)
        {
            if (obj == null || _pool.Contains(obj))
                return;

            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
