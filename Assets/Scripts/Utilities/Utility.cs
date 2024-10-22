using System;
using System.Reflection;

namespace Utilities
{
  public class Utility
  {
    public static void CopyValues<T>(T source, T copy)
    {
      Type type = source.GetType();

      foreach (FieldInfo field in type.GetFields())
      {
        field.SetValue(copy, field.GetValue(source));
      }
    }
  }
}