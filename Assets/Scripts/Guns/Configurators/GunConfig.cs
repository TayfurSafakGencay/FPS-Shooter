using System.Collections;
using Base.Interface;
using Guns.Enum;
using Managers.Manager;
using Surfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Gun", menuName = "Tools/Guns/Gun", order = 0)]
  public class GunConfig : ScriptableObject, System.ICloneable
  {
    // public ImpactType ImpactType;
    public GunType GunType;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    
    [Header("Configurations")]
    public DamageConfig DamageConfig;
    public AmmoConfig AmmoConfig;
    public ShootConfig ShootConfig;
    public TrailConfig TrailConfig;
    public AudioConfig AudioConfig;
    
    private MonoBehaviour _activeMonoBehaviour;
    private Camera _activeCamera;
    private GameObject _model;
    private AudioSource _audioSource;
    
    private float _lastShotTime;
    private float _initialClickTime;
    private float _stopShootingTime;
    private bool _lastFrameWantedToShoot;
    
    private VisualEffect _shootSystem;
    private ObjectPool<TrailRenderer> _trailPool;
    private Player.Player _player;

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour, Camera activeCamera, Player.Player player)
    {
      _activeMonoBehaviour = activeMonoBehaviour;
      _activeCamera = activeCamera;
      _lastShotTime = 0;
      _trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
      
      _model = Instantiate(ModelPrefab, parent, false);
      _model.transform.localPosition = SpawnPoint;
      _model.transform.localRotation = Quaternion.Euler(SpawnRotation);

      _shootSystem = _model.GetComponentInChildren<VisualEffect>();
      _audioSource = _model.GetComponent<AudioSource>();
      _audioSource.outputAudioMixerGroup = SoundManager.Instance.GetMixerGroupToAudioSourceForSFX();

      _player = player;
    }
    
    public void UpdateCamera(Camera activeCamera)
    {
      _activeCamera = activeCamera;
    }

    public void TryToShoot()
    {
      if (Time.time - _lastShotTime - ShootConfig.FireRate > Time.deltaTime)
      {
        float lastDuration = Mathf.Clamp(0, (_stopShootingTime - _initialClickTime), ShootConfig.MaxSpreadTime);
        float lerpTime = (ShootConfig.RecoilRecoverySpeed - (Time.time - _stopShootingTime)) / ShootConfig.RecoilRecoverySpeed;

        _initialClickTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
      }
      
      if (Time.time > ShootConfig.FireRate + _lastShotTime)
      {
        _lastShotTime = Time.time;

        if (AmmoConfig.CurrentClipAmmo == 0)
        {
          AudioConfig.PlayOutOfAmmoClip(_audioSource);
          return;
        }
        
        _shootSystem.Play();
        AudioConfig.PlayShootingClip(_audioSource, AmmoConfig.CurrentClipAmmo);

        Vector3 spreadAmount = ShootConfig.GetSpread(Time.time - _initialClickTime);
        _model.transform.forward += _model.transform.TransformDirection(spreadAmount);
        Vector3 shootDirection = _activeCamera.transform.forward + _activeCamera.transform.TransformDirection(spreadAmount);

        AmmoConfig.CurrentClipAmmo--;
        _player.Fire(ShootConfig.Spread);
        
        if (Physics.Raycast(GetRaycastOrigin(), shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
        {
          _activeMonoBehaviour.StartCoroutine(PlayTrail(_shootSystem.transform.position, hit.point, hit));
        }
        else
        {
          _activeMonoBehaviour.StartCoroutine(PlayTrail(_shootSystem.transform.position,
              _shootSystem.transform.position + shootDirection * TrailConfig.MissDistance,
            new RaycastHit()));
        }
      }
    }
    
    public Vector3 GetRaycastOrigin()
    {
      Vector3 origin = _activeCamera.transform.position + _activeCamera.transform.forward *
        Vector3.Distance(_activeCamera.transform.position, _shootSystem.transform.position);

      return origin;
    }

    public void Tick(bool wantsToShoot)
    {
      _model.transform.localRotation = Quaternion.Lerp(
        _model.transform.localRotation,
        quaternion.Euler(SpawnRotation),
        Time.deltaTime * ShootConfig.RecoilRecoverySpeed
      );
      
      if (wantsToShoot)
      {
        _lastFrameWantedToShoot = true;
        TryToShoot();
      }
      else if (!wantsToShoot && _lastFrameWantedToShoot)
      {
        _stopShootingTime = Time.time;
        _lastFrameWantedToShoot = false;
      }
    }
    
    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
      TrailRenderer trail = _trailPool.Get();
      trail.transform.position = startPoint;
      trail.gameObject.SetActive(true);
      yield return null;
      
      trail.emitting = true;

      float distance = Vector3.Distance(startPoint, endPoint);
      float remainingDistance = distance;
      while (remainingDistance > 0)
      {
        trail.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - remainingDistance / distance));
        remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

        yield return null;
      }

      trail.transform.position = endPoint;

      if (hit.collider != null)
      {
        if (hit.collider.TryGetComponent(out IDamageable damageable))
        {
          Vector3 forceDirection = (hit.point - startPoint).normalized;
          forceDirection.y = 1;
          damageable.TakeDamage(DamageConfig.GetDamage(distance), forceDirection, hit.point, out bool isHeadshot);
          _player.Hit(isHeadshot);
        }
        if (hit.collider.TryGetComponent(out ISurface surface))
        {
          surface.Hit(endPoint, Quaternion.LookRotation(hit.normal));
        }
      }

      yield return new WaitForSeconds(TrailConfig.Duration);
      yield return null;
      trail.emitting = false;
      trail.gameObject.SetActive(false);
      _trailPool.Release(trail);
    }

    private TrailRenderer CreateTrail()
    {
      GameObject instance = new("Bullet Trail");
      TrailRenderer trailRenderer = instance.AddComponent<TrailRenderer>();
      trailRenderer.colorGradient = TrailConfig.Gradient;
      trailRenderer.material = TrailConfig.Material;
      trailRenderer.widthCurve = TrailConfig.WidthCurve;
      trailRenderer.time = TrailConfig.Duration;
      trailRenderer.minVertexDistance = TrailConfig.MinVertexDistance;
      
      trailRenderer.emitting = false;
      trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

      return trailRenderer;
    }
    
    public void Reloading(int section)
    {
      AudioConfig.PlayReloadClip(_audioSource, section);
    }
    
    public bool CanReload()
    {
      return AmmoConfig.CanReload();
    }
    
    public void EndReload()
    {
      AmmoConfig.Reload();
    }

    public object Clone()
    {
      GunConfig clone = CreateInstance<GunConfig>();

      clone.GunType = GunType;
      clone.name = name;
      clone.ModelPrefab = ModelPrefab;
      clone.SpawnPoint = SpawnPoint;
      clone.SpawnRotation = SpawnRotation;
      
      clone.DamageConfig = DamageConfig.Clone() as DamageConfig;
      clone.AmmoConfig = AmmoConfig.Clone() as AmmoConfig;
      clone.ShootConfig = ShootConfig;
      // clone.ShootConfig = ShootConfig.Clone() as ShootConfig;
      clone.TrailConfig = TrailConfig.Clone() as TrailConfig;
      clone.AudioConfig = AudioConfig.Clone() as AudioConfig;

      return clone;
    }
  }
}