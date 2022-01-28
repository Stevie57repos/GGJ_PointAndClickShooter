using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelCameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineDollyCart _cart;
    [SerializeField]
    private float _targetPosition = 1;
    private float _positionPercentage = 0;
    [SerializeField]
    private float _cameraDollySpeed;
    [SerializeField]
    private EnemyDeathEventSO _enemyDeathEventChannel;
    [SerializeField]
    private List<int> targetPositionsList = new List<int>();

    private void OnEnable()
    {
        _enemyDeathEventChannel.EnemyDeathEvent += TriggerNextPosition;
    }

    private void OnDisable()
    {
        
        _enemyDeathEventChannel.EnemyDeathEvent -= TriggerNextPosition;
    }

    private void Start()
    {
        NextCameraPosition();
    }

    private void NextCameraPosition()
    {
        StartCoroutine(MovePositionRoutine(_cart));
    }

    [ContextMenu("Next Camera Position")]
    public void DebugNextCameraPosition()
    {
        StartCoroutine(MovePositionRoutine(_cart));
    }

    private IEnumerator MovePositionRoutine(CinemachineDollyCart cart)
    {   
        

        while(cart.m_Position < _targetPosition)
        {
            float newPosition = Mathf.Lerp(_targetPosition - 1, _targetPosition, _positionPercentage);
            _positionPercentage += _cameraDollySpeed;
            _cart.m_Position = newPosition;
            yield return null;
        }
        _targetPosition += 1;
        _positionPercentage = 0;
    }

    private void TriggerNextPosition(EnemyController enemy)
    {
        NextCameraPosition();
    }
}
