using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private Transform _player;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
         originalColor = _meshRenderer.material.color;
        _unitHealthHandler = GetComponent<UnitHealthHandler>();
        _unitHealthHandler.Setup(_stats);
    }

    public void SetUp(Transform player)
    {
        _player = player;
        transform.LookAt(player);
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
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator DamageVisualRoutine()
    {   
        _meshRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material.color = originalColor;
    }
}
