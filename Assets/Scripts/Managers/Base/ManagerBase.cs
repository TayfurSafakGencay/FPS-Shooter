using System;
using System.Collections.Generic;
using Managers.Loader;
using Managers.Manager;
using UnityEngine;

namespace Managers.Base
{
  public abstract class ManagerBase : MonoBehaviour
  {
    public ManagerKey Key;
    
    public List<GameState> ActivatedGameStates;
    
    protected List<Action> actions = new();

    protected void AddAction(ref Action action, Action method)
    {
      actions.Add(action);
      action += method;
    }

    public abstract void Initialize();

    public virtual void Activate()
    {
      if (ManagerLoader.Instance.CheckManagerActivated(Key)) return;
      
      ManagerLoader.Instance.ActivateManager(Key);
    }

    public void Deactivate()
    {
      for (int i = 0; i < actions.Count; i++)
      {
        actions[i] = null;
      } 
    }

    public virtual void OnDestroy()
    {
      Deactivate();
    }
  }
}