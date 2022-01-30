using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Ally Death Event Channel")]
public class AllyDeathEventSO : ScriptableObject
{
    public UnityAction AllyDeathEvent;
    public void RaiseEvent()
    {
        if (AllyDeathEvent != null)
        {
            AllyDeathEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
