using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Cleared Enemy Wave")]
public class ClearedEnemyWaveEventSO : ScriptableObject
{
    public UnityAction ClearedEnemyWaveEvent;
    public void RaiseEvent()
    {
        if (ClearedEnemyWaveEvent != null)
        {
            ClearedEnemyWaveEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
