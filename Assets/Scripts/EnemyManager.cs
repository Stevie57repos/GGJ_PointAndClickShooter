using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyController _enemyPrefab;
    [SerializeField]
    private Transform _spawn;

    [SerializeField]
    private List<EnemyController> _enemiesList = new List<EnemyController>();

    [SerializeField]
    private EnemyDeathEventSO _enemyDeathChannel;

    [SerializeField]
    private PlayerWinEventSO _playerWinEventChannel;

    [SerializeField]
    private int _numberOfEnemies;

    private void Start()
    {
        SpawnEnemies(_numberOfEnemies);
    }

    private void SpawnEnemies(int Number)
    {
        Vector3 spawnPos = _spawn.position;
        for(int i = 0; i < Number; i++)
        {
            EnemyController enemy = Instantiate(_enemyPrefab);
            enemy.transform.position = spawnPos;
            _enemiesList.Add(enemy);
            spawnPos.x += 2.5f;
        }
    }

    private void OnEnable()
    {
        _enemyDeathChannel.EnemyDeathEvent += EnemyDeathRemoval;
    }

    private void OnDisable()
    {
        _enemyDeathChannel.EnemyDeathEvent -= EnemyDeathRemoval;
    }

    private void EnemyDeathRemoval(EnemyController enemy)
    {
        _enemiesList.Remove(enemy);
        if (_enemiesList.Count == 0)
        {
            _playerWinEventChannel.RaiseEvent();
        }
    }
}
