using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Systems.Chunk
{
  public class EnemySpawner : MonoBehaviour
  {
    private MeshRenderer _meshRenderer;
    private const string _enemyAddressable = "Enemy";
    
    private bool _active = true;

    private void Awake()
    {
      _meshRenderer = GetComponent<MeshRenderer>();
      _meshRenderer.enabled = false;
    }

    public void SpawnEnemy()
    {
      if (!_active) return;
      _active = false;
      
      Addressables.LoadAssetAsync<GameObject>(_enemyAddressable).Completed += (handle) =>
      {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
          GameObject enemy = Instantiate(handle.Result, transform.position, Quaternion.identity, transform);
          enemy.transform.localPosition = new Vector3(0, -0.9f,0);
          enabled = false;
        }
        else
        {
          Debug.LogError("Enemy prefab failed to load!");
        }
      };
    }
  }
}