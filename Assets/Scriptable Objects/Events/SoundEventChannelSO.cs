using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Sound Event Channel")]
public class SoundEventChannelSO : ScriptableObject
{
    public UnityAction<AudioClip, Transform> SoundEvent;
    public void RaiseEvent(AudioClip soundClip, Transform position)
    {
        if (SoundEvent != null)
        {
            SoundEvent.Invoke(soundClip, position);
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
