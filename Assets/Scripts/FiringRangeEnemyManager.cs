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
    private Transform _enemySpawn;
    [SerializeField]
    private Transform _allySpawn;
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private List<EnemyController> _enemiesList = new List<EnemyController>();
    [SerializeField]
    private List<AllyController> _alliesList = new List<AllyController>();
    [SerializeField]
    private List<IInteractable> _listOfInteractables = new List<IInteractable>();
    [SerializeField]
    private EnemyDeathEventSO _enemyDeathChannel;

    [SerializeField]
    private PlayerWinEventSO _playerWinEventChannel;
    [SerializeField]
    private int _numberOfEnemies;
    [SerializeField]
    private int _numberOfAllies;

    private void OnEnable()
    {
        _enemyDeathChannel.EnemyDeathEvent += RemoveEnemy;
    }

    private void OnDisable()
    {
        _enemyDeathChannel.EnemyDeathEvent -= RemoveEnemy;
    }

    private void Start()
    {
        if(_numberOfEnemies != 0)
            SpawnEnemiesInLine(_numberOfEnemies);

        if (_numberOfAllies != 0)
            SpawnAlliesInLine(_numberOfAllies);
    }

    private void SpawnEnemiesInLine(int Number)
    {
        Vector3 spawnPos = _enemySpawn.position;
        for (int i = 0; i < Number; i++)
        {
            EnemyController enemy = Instantiate(_enemyPrefab);
            enemy.transform.position = spawnPos;
            enemy.SetUp(_player);
            _enemiesList.Add(enemy);
            _listOfInteractables.Add(enemy);
            spawnPos.x += 2.5f;
        }
    }

    private void SpawnAlliesInLine(int Number)
    {
        Vector3 spawnPos = _allySpawn.position;
        for (int i = 0; i < Number; i++)
        {
            AllyController ally = Instantiate(_allyPrefab);
            ally.transform.position = spawnPos;
            _alliesList.Add(ally);
            _listOfInteractables.Add(ally);
            spawnPos.x += 2.5f;
        }
    }

    [ContextMenu("Count Interactbles")]
    public void CountInteractablesList()
    {
        print($"Interactables list count is {_listOfInteractables.Count}");
    }

    private void RemoveEnemy(EnemyController enemy)
    {
        _enemiesList.Remove(enemy);
        _listOfInteractables.Remove(enemy);
        if (_enemiesList.Count == 0)
            _playerWinEventChannel.RaiseEvent();
    }
}
