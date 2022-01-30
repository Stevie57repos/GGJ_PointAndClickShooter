using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemyAttackHandler : MonoBehaviour
{
    [SerializeField]
    private bool _attackOnStart;
    void Start()
    {
        if(_attackOnStart)
            GetComponent<EnemyAttackHandler>().Setup(1, GameObject.FindGameObjectWithTag("Player").transform);
    }
}
