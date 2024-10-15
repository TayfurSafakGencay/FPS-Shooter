using UnityEngine.InputSystem;

namespace Managers.Manager.Settings
{
  public partial class SettingsManager
  {
    public void DisableDeviceControls()
    {
      if (Keyboard.current != null) InputSystem.DisableDevice(Keyboard.current);
      if (Mouse.current != null) InputSystem.DisableDevice(Mouse.current);
      if (Gamepad.current != null) InputSystem.DisableDevice(Gamepad.current);
    }
    
    public void EnableDeviceControls()
    {
      if (Keyboard.current != null) InputSystem.EnableDevice(Keyboard.current);
      if (Mouse.current != null) InputSystem.EnableDevice(Mouse.current);
      if (Gamepad.current != null) InputSystem.EnableDevice(Gamepad.current);
    }
  }
}