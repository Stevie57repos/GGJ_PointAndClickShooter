using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[SelectionBase]
public class UnitSOCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private GameObject AllyPrefab;

#if UNITY_EDITOR

    [ContextMenu("Create Enemy Unit")]
    public void CreateEnemyUnitSO()
    {
        StatsSO unit = ScriptableObject.CreateInstance<StatsSO>();
        unit.unitPrefab = EnemyPrefab;
        unit.SpawnPosition = transform.position;
        unit.Type = UnitType.Enemy;        
        unit.HealthStats = GetComponent<EnemyController>().GetStats().HealthStats;
        unit.AttackStats = GetComponent<EnemyController>().GetStats().AttackStats;
        AssetDatabase.CreateAsset(unit, "Assets/Scriptable Objects/Wave 8/Unit.asset");
    }

    [ContextMenu("Create Ally Unit")]
    public void CreateAllyUnitSO()
    {
        StatsSO unit = ScriptableObject.CreateInstance<StatsSO>();
        unit.unitPrefab = AllyPrefab;
        unit.SpawnPosition = transform.position;
        unit.Type = UnitType.Ally;
        unit.HealthStats = GetComponent<AllyController>().GetStats().HealthStats;
        AssetDatabase.CreateAsset(unit, "Assets/Scriptable Objects/Wave 8/Unit.asset");
    }

#endif
}
