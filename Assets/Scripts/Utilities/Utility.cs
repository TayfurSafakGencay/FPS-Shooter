using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Utilities
{
  public static class Utility
  {
    public static void CopyValues<T>(T source, T copy)
    {
      Type type = source.GetType();

      foreach (FieldInfo field in type.GetFields())
      {
        field.SetValue(copy, field.GetValue(source));
      }
    }

    public static async Task Delay(float seconds)
    {
      int time = (int)(seconds * 1000);
      await Task.Delay(time);
    }
  }
}