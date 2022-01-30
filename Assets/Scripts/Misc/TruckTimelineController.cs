using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TruckTimelineController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _director;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _director.Play();
            print($"called");
        }
    }
}
