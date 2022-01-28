using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringRangeEnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyController _enemyPrefab;
    [SerializeField]
    private AllyController _allyPrefab;
    [SerializeField]
    private Transform _spawn;

    [SerializeField]
    private List<EnemyController> _enemiesList = new List<EnemyController>();
    [SerializeField]
    private List<AllyController> _alliesList = new List<AllyController>();

    [SerializeField]
    private EnemyDeathEventSO _enemyDeathChannel;

    [SerializeField]
    private PlayerWinEventSO _playerWinEventChannel;
    // Update is called once per frame
    [SerializeField]
    private int _numberOfEnemies;
    [SerializeField]
    private int _numberOfAllies;

    private void Start()
    {
        if(_numberOfEnemies != 0)
            SpawnEnemiesInLine(_numberOfEnemies);

        if (_numberOfAllies != 0)
            SpawnAlliesInLine(_numberOfAllies);
    }

    private void SpawnEnemiesInLine(int Number)
    {
        Vector3 spawnPos = _spawn.position;
        for (int i = 0; i < Number; i++)
        {
            EnemyController enemy = Instantiate(_enemyPrefab);
            enemy.transform.position = spawnPos;
            _enemiesList.Add(enemy);
            spawnPos.x += 2.5f;
        }
    }

    private void SpawnAlliesInLine(int Number)
    {
        Vector3 spawnPos = _spawn.position;
        for (int i = 0; i < Number; i++)
        {
            AllyController ally = Instantiate(_allyPrefab);
            ally.transform.position = spawnPos;
            _alliesList.Add(ally);
            spawnPos.x += 2.5f;
        }
    }
}
