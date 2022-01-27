using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Player Win Event Channel")]
public class PlayerWinEventSO : ScriptableObject
{
    public UnityAction PlayerWinEvent;
    public void RaiseEvent()
    {
        if (PlayerWinEvent != null)
        {
            PlayerWinEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
