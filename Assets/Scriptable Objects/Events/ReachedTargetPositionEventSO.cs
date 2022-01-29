using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events/Reached Target Position Event Channel")]
public class ReachedTargetPositionEventSO : ScriptableObject
{
    public UnityAction ReachedTargetPosition;
    public void RaiseEvent()
    {
        if (ReachedTargetPosition != null)
        {
            ReachedTargetPosition.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
