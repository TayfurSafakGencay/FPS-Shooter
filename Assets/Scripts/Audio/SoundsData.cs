using System.Collections.Generic;
using Managers.Manager;
using UnityEngine;

namespace Audio
{
  [CreateAssetMenu(fileName = "Audio Data", menuName = "Tools/Audio/Create Audio Data", order = 0)]
  public class SoundsData : ScriptableObject
  {
    [SerializeField]
    private List<SoundDTO> _audioClips;

    private Dictionary<SoundKey, SoundDTO> _audios;

    public void Initialize()
    {
      _audios = new Dictionary<SoundKey, SoundDTO>();

      for (int index = 0; index < _audioClips.Count; index++)
      {
        SoundDTO soundDTO = _audioClips[index];
        _audios.TryAdd(soundDTO.SoundKey, soundDTO);
      }
    }

    public SoundDTO GetSound(SoundKey soundKey)
    {
      return _audios[soundKey];
    }

    private void OnValidate()
    {
      for (int i = 0; i < _audioClips.Count; i++)
      {
        _audioClips[i].Name = _audioClips[i].SoundKey.ToString();
      }
    }
  }
}