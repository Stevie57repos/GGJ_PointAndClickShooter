using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Player Lose Event Channel")]
public class PlayerLoseEventChannel : ScriptableObject
{
    public UnityAction PlayerLoseEvent;
    public void RaiseEvent()
    {
        if (PlayerLoseEvent != null)
        {
            PlayerLoseEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
