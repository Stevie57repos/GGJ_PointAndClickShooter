using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(UnitHealthHandler))]
public class EnemyController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Canvas _UICanvas;   
    private UnitHealthHandler _unitHealthHandler;
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private StatsSO _stats;
    Color originalColor;
    [SerializeField]
    private EnemyDeathEventSO _enemyDeathEventChannel;

    [SerializeField]
    private EnemyAttackHandler _attackHandler;
    [SerializeField]
    private DissolveObject _dissolver;
    [SerializeField]
    private float _dissolveTime;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
         originalColor = _meshRenderer.material.color;
        _unitHealthHandler = GetComponent<UnitHealthHandler>();
        _unitHealthHandler.Setup(_stats);         
    }

    private void Start()
    {
        _dissolver.DissolveIn(_dissolveTime);
    }

    public void SetUp(Transform player)
    {
        _attackHandler.Setup(_stats.AttackStats.Damage, player);        
    }

    private void OnEnable()
    {
        _UICanvas.worldCamera = Camera.main;
    }

    public void LeftClick(float heal)
    {
        _unitHealthHandler.TakeDamage(heal);
        StartCoroutine(DamageVisualRoutine());   
    }

    public void RightClick(float damage)
    {
        bool isAlive = _unitHealthHandler.TakeDamage(damage);
        if (isAlive)
        {
            StartCoroutine(DamageVisualRoutine());
        }
        else
        {
            _enemyDeathEventChannel.RaiseEvent(this);
            //this.gameObject.SetActive(false);
            EnemyDeath();
        }
    }

    private IEnumerator DamageVisualRoutine()
    {   
        _meshRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material.color = originalColor;
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public StatsSO GetStats()
    {
        return _stats;
    }

    private void EnemyDeath()
    {
        _UICanvas.gameObject.SetActive(false);
        StartCoroutine(EnemyDeathRoutine(_dissolveTime));
    }

    private IEnumerator EnemyDeathRoutine(float dissolveTime)
    {
        _dissolver.DissolveOut(dissolveTime);
        yield return new WaitForSeconds(dissolveTime);
        Destroy(this.gameObject);
    }

    [ContextMenu("Take Damage 1")]
    public void TakeDamage1()
    {
        RightClick(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OutsideFOV"))
        {
            _unitHealthHandler.TakeDamage(_unitHealthHandler.CurrentHealth());
        }
    }
}
