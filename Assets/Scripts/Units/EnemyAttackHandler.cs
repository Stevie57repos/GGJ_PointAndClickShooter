using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetingReticle;
    private float _startAttackTime;
    [SerializeField]
    private float _startSize;
    [SerializeField]
    private float _attackChargeDuration;
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private Color _startColor;
    [SerializeField]
    private Color _endColor;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private EnemyProjectile _enemyProjectile;
    [SerializeField]
    private float _attackDamage;
    private Transform _player;
    [SerializeField]
    private Transform _bulletSpawnPoint;
    [SerializeField]
    private float _projectileSpeed = 1f;
    [SerializeField]
    private float _targetRangeCheck;
    [SerializeField]
    private LayerMask _allyUnitsLayer;
    private Transform _target;
    [SerializeField]
    Collider[] _colliders;

    private void OnEnable()
    {
        if(_meshRenderer == null)
            _meshRenderer = _targetingReticle.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Setup(1, GameObject.FindGameObjectWithTag("Player").transform);
    }

    public void Setup(float damage, Transform player)
    {
        _attackDamage = damage;
        _player = player;
        SetTargetReticle();
        FindTarget();
        AttackTarget();
    }

    // Find Available Target prioritizing ally units
    private void FindTarget()
    {      
        _colliders = Physics.OverlapSphere(transform.position, _targetRangeCheck, _allyUnitsLayer);

        if (_colliders.Length == 0)
        {
            transform.LookAt(_player);
            _target = _player;
            print($"no targets found");
        }
        else
        {
            Transform allyUnit = _colliders[0].GetComponent<AllyController>().transform;
            transform.LookAt(allyUnit);
            _target = allyUnit;
        }
    }

    private void SetTargetReticle()
    {
        _targetingReticle.transform.localScale = new Vector3(_startSize, _startSize, _startSize);
        Vector3 targetDireciton = (_player.position - transform.position).normalized * 1f;
        _targetingReticle.transform.position += targetDireciton;
    }

    public void AttackTarget()
    {
        _meshRenderer.material.color = _startColor;
        StartCoroutine(BeginAttackRoutine());
    }

    private IEnumerator TargetingRoutine(float waitDuration)
    {
        while(Time.time < _startAttackTime + waitDuration)
        {
            float chargePercentage = (Time.time - _startAttackTime) / _attackChargeDuration;

            float newSize = Mathf.Lerp(_startSize, 2, chargePercentage);            
            _targetingReticle.transform.localScale = new Vector3(newSize,newSize,newSize);

            Color newColor = Color.Lerp(_startColor, _endColor, chargePercentage);
            _meshRenderer.material.color = newColor;
            yield return null;
        }
        EnemyProjectile projectile = Instantiate(_enemyProjectile);
        projectile.transform.position = _bulletSpawnPoint.position;
        projectile.LaunchProjectile(_target.position, _attackDamage, _projectileSpeed);
    }

    private IEnumerator BeginAttackRoutine()
    {
        yield return new WaitForSeconds(_attackDelay);
        _startAttackTime = Time.time;
        StartCoroutine(TargetingRoutine(_attackChargeDuration));
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    private void OnDrawGizmos()
    {
        Color sphere = Color.yellow;
        sphere.a = 0.2f;
        Gizmos.color = sphere;
        Gizmos.DrawSphere(transform.position, _targetRangeCheck);
    }
}
