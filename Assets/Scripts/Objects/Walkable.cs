using System;
using Base.Interface;
using UnityEngine;

namespace Objects
{
  [Serializable]
  public class Walkable : MonoBehaviour, IWalkable
  {
    [SerializeField]
    private WalkableType _walkableType;

    public WalkableType WalkableType
    {
      get => _walkableType;
      set => _walkableType = value;
    }
  }
}