using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetingReticle;
    private float _startAttackTime;

    public void AttackPlayer(float waitDuration)
    {
        _startAttackTime = Time.time;
        StartCoroutine(TargetingRoutine(waitDuration));
    }

    private IEnumerator TargetingRoutine(float waitDuration)
    {
        while(Time.time < _startAttackTime + waitDuration)
        {
            yield return null;
        }
    }

}
