using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Stop Sound Event Channel")]
public class StopSoundEventChannelSO : ScriptableObject
{
    public UnityAction StopSoundEvent;
    public void RaiseEvent()
    {
        if (StopSoundEvent != null)
        {
            StopSoundEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
