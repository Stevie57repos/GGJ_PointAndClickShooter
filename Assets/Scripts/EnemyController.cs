using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthHandler))]
public class EnemyController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Canvas _UICanvas;   
    private HealthHandler _healthHandler;
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private StatsSO _stats;
    Color originalColor;
    [SerializeField]
    private EnemyDeathEventSO _enemyDeathEventChannel;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
         originalColor = _meshRenderer.material.color;
        _healthHandler = GetComponent<HealthHandler>();
        _healthHandler.Setup(_stats);
    }

    private void OnEnable()
    {
        _UICanvas.worldCamera = Camera.main;
    }

    public void LeftClick(float heal)
    {
        _healthHandler.TakeDamage(heal);
        StartCoroutine(DamageVisualRoutine());   
    }

    public void RightClick(float damage)
    {
        bool isAlive = _healthHandler.TakeDamage(damage);
        if (isAlive)
        {
            StartCoroutine(DamageVisualRoutine());
        }
        else
        {
            _enemyDeathEventChannel.RaiseEvent(this);
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator DamageVisualRoutine()
    {   
        _meshRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.25f);
        _meshRenderer.material.color = originalColor;
    }
}
