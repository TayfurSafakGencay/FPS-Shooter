using UnityEngine;

namespace Surfaces
{
  public class Surface : MonoBehaviour, ISurface
  {
    public SurfaceConfig Config;

    public void Hit(Vector3 position, Quaternion rotation)
    {
      Config.PlayParticle(position, rotation);
    }
  }
}