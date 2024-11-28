using System;
using System.Collections.Generic;
using Guns.Enum;
using LootSystem;
using UnityEngine;

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
    
    private void AddWeapon(Loot loot)
    {
      switch (loot.Key)
      {
        case LootKey.AK47:
          _player.GetPlayerGunSelector().EquipGun(GunType.AK47);
          break;
        case LootKey.M4:
          _player.GetPlayerGunSelector().EquipGun(GunType.M4);
          break;
        default:
          throw new ArgumentOutOfRangeException();
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
      
      _player.GetPlayerGunSelector().AddAmmo(gunType, loot.Quantity);
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