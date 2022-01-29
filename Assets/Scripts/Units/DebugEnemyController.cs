using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemyController : MonoBehaviour
{
    private EnemyController _enemyController;
    [SerializeField]
    private Transform _target;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyController.SetUp(_target) ;
    }

}
