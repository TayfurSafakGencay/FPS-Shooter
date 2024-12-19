using DG.Tweening;
using Enemy.Zombie;
using UnityEngine;

namespace Actor
{
  [RequireComponent(typeof(SphereCollider))]
  [RequireComponent(typeof(AudioSource))]
  public class PlayerRadar : MonoBehaviour
  {
    private SphereCollider _sphereCollider;

    private AudioSource _audioSource;

    private const float _targetRadius = 100;

    private const float _duration = 10;

    private void Awake()
    {
      _sphereCollider = GetComponent<SphereCollider>();
      _audioSource = GetComponent<AudioSource>();

      _sphereCollider.enabled = false;
      _audioSource.loop = true;
      _audioSource.enabled = false;
    }

    public void StartRadar()
    {
      _audioSource.time = 0;
      _sphereCollider.radius = 0;

      _sphereCollider.enabled = true;
      _audioSource.enabled = true;
      _audioSource.Play();

      DOTween.To(() => _sphereCollider.radius, x => _sphereCollider.radius = x, _targetRadius, _duration)
        .SetEase(Ease.InOutQuad);
    }

    public void StopRadar()
    {
      _sphereCollider.enabled = false;
      _audioSource.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.TryGetComponent(out IOutlineable outlineable))
      {
        outlineable?.OutlineMesh();
      }
    }
  }
}