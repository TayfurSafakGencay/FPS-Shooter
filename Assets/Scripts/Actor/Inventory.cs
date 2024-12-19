using System;
using System.Collections.Generic;
using Guns.Enum;
using LootSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Actor
{
  public class Inventory : MonoBehaviour
  {
    private Dictionary<LootKey, ItemVo> Items = new();

    private Player _player;

    private void Awake()
    {
      _player = GetComponent<Player>();
    }

    public void AddItemToInventory(Loot loot)
    {
      switch (loot.Type)
      {
        case LootType.Weapon:
          AddWeapon(loot);
          break;
        case LootType.Ammo:
          AddAmmo(loot);
          break;
        case LootType.Consumable:
          AddConsumable(loot);
          break;
      }
    }
    
    private async void AddWeapon(Loot loot)
    {
      GunType gunType;
      switch (loot.Key)
      {
        case LootKey.AK47:
           gunType = GunType.AK47;
          break;
        case LootKey.M4:
          gunType = GunType.M4;
          break;
        default:
          return;
      }
      
      CheckIsThereAmmoInInventory(gunType);

      if (!IsRadarUsed)
      {
        _player.GetPlayerGunSelector().EquipGun(gunType);
        return;
      }
      
      UseRadar();
      await Utility.Delay(0.85f);
      _player.GetPlayerGunSelector().EquipGun(gunType);
    }
    
    private void CheckIsThereAmmoInInventory(GunType gunType)
    {
      switch (gunType)
      {
        case GunType.AK47:
          if (Items.TryGetValue(LootKey.Ammo_AK47, out ItemVo akAmmo))
          {
            _player.GetPlayerGunSelector().AddAmmo(gunType, akAmmo.Quantity);
            return;
          }
          break;
        case GunType.M4:
          if (Items.TryGetValue(LootKey.Ammo_M4, out ItemVo m4Ammo))
          {
            _player.GetPlayerGunSelector().AddAmmo(gunType, m4Ammo.Quantity);
            return;
          }
          break;
      }
      if (Items.ContainsKey(gunType switch
      {
        
        GunType.AK47 => LootKey.Ammo_AK47,
        GunType.M4 => LootKey.Ammo_M4,
        _ => throw new ArgumentOutOfRangeException()
      }))
      {
        return;
      }
    }
    
    private void AddAmmo(Loot loot)
    {
      GunType gunType = loot.Key switch
      {
        LootKey.Ammo_AK47 => GunType.AK47,
        LootKey.Ammo_M4 => GunType.M4,
        _ => throw new ArgumentOutOfRangeException()
      };
      
      bool hasTheGun = _player.GetPlayerGunSelector().AddAmmo(gunType, loot.Quantity);

      if (!hasTheGun)
      {
        AddConsumable(loot);
      }
    }
    
    private void AddConsumable(Loot loot)
    {
      if (!Items.ContainsKey(loot.Key))
      {
        Items.Add(loot.Key, new ItemVo
        {
          Loot = loot,
          Quantity = loot.Quantity
        });
      }
      else
      {
        ItemVo item = Items[loot.Key];

        if (item.Quantity + loot.Quantity <= loot.MaxQuantity)
        {
          item.Quantity += loot.Quantity;
        }
        else
        {
          item.Quantity = loot.MaxQuantity;
        }
      }
    }
    
    public async void UseConsumable(LootKey key)
    {
      if (!Items.ContainsKey(key)) return;
      
      if (key == LootKey.Pill)
      {
        if (_player.GetPlayerHealth().IsFullHealth()) return;

        if (IsRadarUsed)
        {
          UseRadar();
          await Utility.Delay(0.85f);
        }
        
        await _player.GetPlayerGunSelector().EmptyHand();
        
        _player.GetPlayerAnimationController().Pill();
      }
      
      ItemVo item = Items[key];
      
      item.Quantity--;
      
      if (item.Quantity == 0)
      {
        Items.Remove(key);
      }
    }
    
    [HideInInspector]
    public bool IsRadarUsed;
    public async void UseRadar()
    {
      if (!Items.ContainsKey(LootKey.Radar)) return;

      if (!IsRadarUsed)
      {
        await _player.GetPlayerGunSelector().EmptyHand();
        IsRadarUsed = true;
        _player.GetPlayerAnimationController().Radar();
      }
      else
      {
        IsRadarUsed = false;
        _player.GetPlayerAnimationController().PutRadarToBackpack();
      }
    }

    public object GetConsumableCount(LootKey lootKey)
    {
      if (!Items.ContainsKey(lootKey)) return 0;
      
      return Items[lootKey].Quantity;
    }
  }

  public class ItemVo
  {
    public Loot Loot;

    public int Quantity;
  }
}