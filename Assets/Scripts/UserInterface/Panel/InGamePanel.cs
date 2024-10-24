﻿using Player.Gun;
using TMPro;
using UnityEngine;

namespace UserInterface.Panel
{
  public class InGamePanel : MonoBehaviour
  {
    [SerializeField]
    private PlayerGunSelector _playerGunSelector;

    [SerializeField]
    private TextMeshProUGUI _ammoText;

    private void Awake()
    {
      _playerGunSelector = FindObjectOfType<PlayerGunSelector>();
    }

    private void Update()
    {
      _ammoText.SetText($"{_playerGunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo} " + "/" + _playerGunSelector.ActiveGun.AmmoConfig.CurrentAmmo);
    }
  }
}