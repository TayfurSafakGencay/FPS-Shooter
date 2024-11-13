using UnityEngine;

namespace LootSystem
{
  public class Loot : MonoBehaviour
  {
    public LootKey Key;
    
    public LootType Type;

    public string Text;

    public int MaxQuantity;

    public int Quantity;

    public void Destroy()
    {
      Destroy(gameObject);
    }
  }

  public enum LootType
  {
    Weapon,
    Ammo,
    Consumable,
  }
  
  public enum LootKey
  {
    Pill,
    AK47,
    Ammo_AK47,
    M4,
    Ammo_M4,
  }
}