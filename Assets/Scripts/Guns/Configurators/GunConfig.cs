using System.Collections;
using Guns.Enum;
using UnityEngine;
using UnityEngine.Pool;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Gun", menuName = "Tools/Guns/Gun", order = 0)]
  public class GunConfig : ScriptableObject
  {
    // public ImpactType ImpactType;
    public GunType GunType;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    
    public ShootConfig ShootConfig;
    public TrailConfig TrailConfig;

    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _model;
    private float _lastShotTime;
    private ParticleSystem _shootSystem;
    private ObjectPool<TrailRenderer> _trailPool;

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
      _activeMonoBehaviour = activeMonoBehaviour;
      _lastShotTime = 0;
      _trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
      
      _model = Instantiate(ModelPrefab, parent, false);
      _model.transform.localPosition = SpawnPoint;
      _model.transform.localRotation = Quaternion.Euler(SpawnRotation);

      _shootSystem = _model.GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot()
    {
      if (Time.time > ShootConfig.FireRate + _lastShotTime)
      {
        _lastShotTime = Time.time;
        _shootSystem.Play();
        
        float spreadX = Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x);
        float spreadY = Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y);
        float spreadZ = Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z);
        Vector3 shootDirection = _shootSystem.transform.forward + new Vector3(spreadX, spreadY, spreadZ);
        shootDirection.Normalize();

        if (Physics.Raycast(_shootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
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

      // if (hit.collider != null)
      // {
      //   SurfaceManager.Instance.HandleImpact(hit.transform.gameObject, endPoint, hit.normal, ImpactType, 0);
      // }

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
  }
}