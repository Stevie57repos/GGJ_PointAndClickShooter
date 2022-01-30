using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitHealthHandler))]
public class AllyController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Canvas _UICanvas;
    private UnitHealthHandler _unitHealthHandler;
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private StatsSO _stats;
    Color originalColor;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        originalColor = _meshRenderer.material.color;
        _unitHealthHandler = GetComponent<UnitHealthHandler>();
        _unitHealthHandler.Setup(_stats);
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
            // ally death;
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator DamageVisualRoutine()
    {
        _meshRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material.color = originalColor;
    }

    public void TakeDamage(float damage)
    {
        _unitHealthHandler.TakeDamage(damage);
    }
}
