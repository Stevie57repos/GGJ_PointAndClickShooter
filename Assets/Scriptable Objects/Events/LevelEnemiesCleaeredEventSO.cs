using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Cleared Level Enemies")]
public class LevelEnemiesCleaeredEventSO : ScriptableObject
{
    public UnityAction ClearedlevelEnemiesEvent;
    public void RaiseEvent()
    {
        if (ClearedlevelEnemiesEvent != null)
        {
            ClearedlevelEnemiesEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
