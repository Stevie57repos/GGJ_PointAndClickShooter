using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Enemy Death Event Channel")]
public class EnemyDeathEventSO : ScriptableObject
{
    public UnityAction<EnemyController> EnemyDeathEvent;
    public void RaiseEvent(EnemyController enemy)
    {
        if(EnemyDeathEvent != null)
        {
            EnemyDeathEvent.Invoke(enemy);
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}

