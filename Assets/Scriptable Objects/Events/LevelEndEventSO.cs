using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Level End Event SO")]
public class LevelEndEventSO : ScriptableObject
{
    public UnityAction LevelEndEvent;
    public void RaiseEvent()
    {
        if (LevelEndEvent != null)
        {
            LevelEndEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
