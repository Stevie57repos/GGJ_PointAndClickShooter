using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<EnemyController> _enemiesList = new List<EnemyController>();
    [SerializeField]
    private List<UnitWaveSO> _unitWavesList = new List<UnitWaveSO>();
    [Header("Event Channels")]
    [SerializeField]
    private EnemyDeathEventSO _enemyDeathEventChannel;
    [SerializeField]
    private ClearedEnemyWaveEventSO _clearedEnemyWaveEventChannel;
    [SerializeField]
    private bool _isWaveSpawningComplete;

    private Transform _player;

    private void OnEnable()
    {
        _enemyDeathEventChannel.EnemyDeathEvent += RemoveEnemy;
    }

    private void OnDisable()
    {
        _enemyDeathEventChannel.EnemyDeathEvent -= RemoveEnemy;
    }

    public void Setup(Transform player)
    {
        _player = player;
    }

    public void SpawnUnits(int currentPosition)
    {
        if (currentPosition == _unitWavesList.Count) return;
        List<StatsSO> unitList = _unitWavesList[currentPosition].UnitList;
        _isWaveSpawningComplete = false;
        StartCoroutine(SpawnUnitsRoutine(unitList));
    }

    private IEnumerator SpawnUnitsRoutine(List<StatsSO> unitList)
    {
        for(int i = 0; i < unitList.Count; i++)
        {
            StatsSO unitData = unitList[i];           
            var unit = Instantiate(unitData.unitPrefab);
            unit.transform.position = unitData.SpawnPosition;
            if(unitData.Type == UnitType.Enemy)
            {
                _enemiesList.Add(unit.transform.GetComponent<EnemyController>());
                unit.transform.GetComponent<EnemyController>().SetUp(_player);
            }
            yield return new WaitForSeconds(unitData.NextSpawnDelay);
        }
        _isWaveSpawningComplete = true;
    }

    private void RemoveEnemy(EnemyController enemy)
    {
        _enemiesList.Remove(enemy);
        if(_enemiesList.Count == 0 && _isWaveSpawningComplete)
        {
            _clearedEnemyWaveEventChannel.RaiseEvent();
        }
    }
}
