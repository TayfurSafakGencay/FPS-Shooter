
namespace Managers.Manager.Settings
{
  public partial class SettingsManager
  {
    public float MasterVolume { get; private set; }
    
    public void SetMasterVolume(float volume)
    {
      MasterVolume = volume;
      
      SoundManager.Instance.SetMasterVolume(volume);
    }

    public float MusicVolume { get; private set; }
    
    public void SetMusicVolume(float volume)
    {
      MusicVolume = volume;
      
      SoundManager.Instance.SetMusicVolume(volume);
    }

    public float SFXVolume { get; private set; }

    public void SetSFXVolume(float volume)
    {
      SFXVolume = volume;
      
      SoundManager.Instance.SetSFXVolume(volume);
    }
    
    public float UIVolume { get; private set; }
    
    public void SetUIVolume(float volume)
    {
      UIVolume = volume;
      
      SoundManager.Instance.SetUIVolume(volume);
    }
  }
}