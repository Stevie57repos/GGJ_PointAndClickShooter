using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/EnemyStat")]
[System.Serializable]
public class StatsSO : ScriptableObject
{
    public UnitType Type;
    public GameObject unitPrefab;
    public Vector3 SpawnPosition;
    public float NextSpawnDelay;
    public HealthStats HealthStats;
    public AttackStats AttackStats;
}

[System.Serializable]
public struct HealthStats
{
    public float maxHealth;
}

[System.Serializable]
public struct AttackStats
{
    public float Damage;
    public float AttackChargeTime;
}

public enum UnitType { Enemy, Ally };