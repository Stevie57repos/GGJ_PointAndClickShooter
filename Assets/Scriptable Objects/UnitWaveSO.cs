using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/EnemyWave")]
[System.Serializable]
public class UnitWaveSO : ScriptableObject
{
    public List<StatsSO> UnitList = new List<StatsSO>();   
}
