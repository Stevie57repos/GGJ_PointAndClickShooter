using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AllyHealthHandler))]
public class AllyController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Canvas _UICanvas;
    private AllyHealthHandler _allyHealthHandler;

    private MeshRenderer _meshRenderer;
    [SerializeField]
    private StatsSO _stats;
    Color originalColor;

    [SerializeField]
    private DissolveObject _dissolver;
    [SerializeField]
    private float _dissolveTime = 5f;

    [Header("Event Channels")]
    [SerializeField]
    private AllyDeathEventSO _allyDeathEventChannel;
    

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        originalColor = _meshRenderer.material.color;
        _allyHealthHandler = GetComponent<AllyHealthHandler>();
        _allyHealthHandler.Setup(_stats);
    }

    private void Start()
    {
        _dissolver.DissolveIn(_dissolveTime);
    }

    private void OnEnable()
    {
        _UICanvas.worldCamera = Camera.main;
    }

    public void LeftClick(float heal)
    {
        //TakeDamage(heal);
        //print($"called");
        //bool isAlive = _allyHealthHandler.TakeDamage(-heal);
        //if (isAlive)
        //{
        //    StartCoroutine(DamageVisualRoutine());
        //}
        //else
        //{
        //    _allyDeathEventChannel.RaiseEvent();
        //    _dissolver.DissolveOut(_dissolveTime);
        //    StartCoroutine(AllyDeathRoutine());
        //}
    }

    public void RightClick(float damage)
    {
        TakeDamage(damage);
        print($"called");
        //bool isAlive = _allyHealthHandler.TakeDamage(damage);
        //if (isAlive)
        //{
        //    StartCoroutine(DamageVisualRoutine());
        //}
        //else
        //{
        //    // ally death;
        //    this.gameObject.SetActive(false);
        //}
    }

    private IEnumerator DamageVisualRoutine()
    {
        _meshRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material.color = originalColor;
    }

    public void TakeDamage(float damage)
    {
        bool isAlive = _allyHealthHandler.TakeDamage(damage);
        if (!isAlive)
        {
            _allyDeathEventChannel.RaiseEvent();
            _dissolver.DissolveOut(_dissolveTime);
            StartCoroutine(AllyDeathRoutine());
        }        
    }

    private IEnumerator AllyDeathRoutine()
    {
        yield return new WaitForSeconds(_dissolveTime);
        Destroy(gameObject);
    }

    public StatsSO GetStats()
    {
        return _stats;
    }

}
