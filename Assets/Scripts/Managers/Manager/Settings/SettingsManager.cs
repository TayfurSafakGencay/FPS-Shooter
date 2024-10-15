﻿using UnityEngine;

namespace Managers.Manager.Settings
{
  public partial class SettingsManager : MonoBehaviour
  {
    public static SettingsManager Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }
  }
}