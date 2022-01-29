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
        AssetDatabase.CreateAsset(unit, "Assets/Scriptable Objects/Wave 2/newUnit.asset");
    }

#endif
}
