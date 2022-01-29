using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private float _projectileDamage;

    private void OnEnable()
    {
        if(_rigidBody == null)
            _rigidBody = GetComponent<Rigidbody>();
    }
    public void LaunchProjectile(Vector3 targetPos, float damage, float projectileSpeed )
    {
        Vector3 direction = (targetPos - transform.position).normalized * projectileSpeed;
        _projectileDamage = damage;
        _rigidBody.AddForce(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerController>().TakeDamage(_projectileDamage);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Ally"))
        {
            other.transform.GetComponent<AllyController>().TakeDamage(_projectileDamage);
            Destroy(this.gameObject);
        }
    }
}
