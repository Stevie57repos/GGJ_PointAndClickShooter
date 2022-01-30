using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesController : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> _boxesList = new List<Rigidbody>();
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float burstStrength;

    [ContextMenu("Burst Boxes")]
    public void BurstBoxes()
    {
        Vector3 direction = _target.position - transform.position;

        foreach(var box in _boxesList)
        {
            box.AddForce(direction.normalized * burstStrength);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Truck"))
            BurstBoxes();
    }

}
