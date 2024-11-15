namespace Base.Interface
{
  public interface IWalkable
  {
    public WalkableType WalkableType { get; set; }
  }
  
  public enum WalkableType
  {
    Terrain,
    Water,
    Wood
  }
}