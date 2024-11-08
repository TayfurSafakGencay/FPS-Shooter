using System.Collections.Generic;
using System.Threading.Tasks;
using Managers.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers.Manager
{
  public class DataManager : ManagerBase
  {
    public static DataManager Instance;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChange);
    }

    private void OnGameStateChange()
    {
      if (ActivatedGameStates.Contains(GameManager.Instance.CurrentGameState))
      {
        Activate();
      }
    }

    public async Task<T> LoadData<T>(DataKey dataKey) where T : ScriptableObject
    {
      return await LoadAddressableAsync<T>(dataKey.ToString());
    }

    private async Task<T> LoadAddressableAsync<T>(string addressableKey) where T : ScriptableObject
    {
      AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(addressableKey);
      await handle.Task;

      if (handle.Status == AsyncOperationStatus.Succeeded)
      {
        return handle.Result;
      }

      Debug.LogError($"Failed to load asset of type {typeof(T)} with key: {addressableKey}");
      return null;
    }

    public async Task<IList<T>> LoadDataAsList<T>(DataKey dataKey) where T : ScriptableObject
    {
      return await LoadAssetListAsync<T>(dataKey.ToString());
    }

    private async Task<IList<T>> LoadAssetListAsync<T>(string addressableKey) where T : ScriptableObject
    {
      AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetAsync<IList<T>>(addressableKey);
      await handle.Task;

      if (handle.Status == AsyncOperationStatus.Succeeded)
      {
        return handle.Result;
      }

      Debug.LogError($"Failed to load list of assets of type {typeof(T)} with key: {addressableKey}");
      return null;
    }
  }

  public enum DataKey
  {
    ManagerConfig = 0,
  }
}