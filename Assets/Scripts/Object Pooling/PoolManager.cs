using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private EnemyProjectile _projectilePrefab;
    [SerializeField]
    private int _projectilePoolAmount = 10;

    private void Awake()
    {
        PoolSystem.CreatePool(_projectilePrefab, _projectilePoolAmount);
    }
}
