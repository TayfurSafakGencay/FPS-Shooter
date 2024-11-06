using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Actor
{
  public class PlayerSpawner : MonoBehaviour
  {
    private Transform _playerSpawnPoint;

    private const string _playerKey = "Player";
    private void Awake()
    {
      _playerSpawnPoint = transform;
      
      Addressables.InstantiateAsync(_playerKey, _playerSpawnPoint.position, Quaternion.identity);
    }
  }
}