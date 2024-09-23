using UnityEngine;

namespace Surfaces
{
  public interface ISurface
  {
    void Hit(Vector3 position, Quaternion rotation);
  }
}