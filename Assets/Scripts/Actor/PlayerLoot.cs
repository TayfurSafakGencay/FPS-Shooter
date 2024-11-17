using LootSystem;
using UnityEngine;

namespace Actor
{
  public class PlayerLoot : MonoBehaviour
  {
    [SerializeField]
    private Camera _camera;
    
    private bool _scanning;
    
    private Loot _lootItem;
    
    private Player _player;

    private void Awake()
    {
      _player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("LootArea"))
      {
        _scanning = true;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("LootArea"))
      {
        _scanning = false;
      }
    }

    private void Update()
    {
      if (!_scanning) return;

      if (Physics.Raycast(_camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)),
            out RaycastHit hit, 3f, LayerMask.GetMask("Loot")))
      {
        if (_lootItem != null)
        {
          if (_lootItem.gameObject == hit.collider.gameObject) return;
        }
        
        TargetLoot(hit);
      }
      else
      {
        NoLoot();
      }
    }
    
    public void NoLoot()
    {
      _lootItem = null;
      
      _player.GetPlayerScreenPanel().DisableLootText();
    }
    
    public void TargetLoot(RaycastHit hit)
    {
      _lootItem = hit.collider.GetComponent<Loot>();
      
      _player.GetPlayerScreenPanel().SetLootText(_lootItem.Text);
    }

    public void TakeLootItem()
    {
      if (_lootItem == null) return;
      
      _player.GetInventory().AddItemToInventory(_lootItem);
      _lootItem.Destroy();
      _lootItem = null;
    }
  }
}