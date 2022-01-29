using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelCameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineDollyCart _cart;
    [SerializeField]
    private int _targetPosition;
    private int _previousPosition;
    private int _currentListPositionTracker;
    private float _positionPercentage = 0;
    [SerializeField]
    private float _cameraDollySpeed;
    [SerializeField]
    private EnemyDeathEventSO _enemyDeathEventChannel;
    [SerializeField]
    private List<int> _targetPositionsList = new List<int>();
    [Header("Event Channels")]
    [SerializeField]
    private PlayerWinEventSO _playerWinEventChannel;
    [SerializeField]
    private ReachedTargetPositionEventSO _reachedTargetPositionEventChannel;
    private void Awake()
    {
        _positionPercentage = 0;
        _previousPosition = 0;
        _currentListPositionTracker = 0;
        _targetPosition = _targetPositionsList[_currentListPositionTracker];
    }

    public void NextCameraPosition()
    {

        StartCoroutine(MovePositionRoutine());
    }

    [ContextMenu("Next Camera Position")]
    public void DebugNextCameraPosition()
    {
        StartCoroutine(MovePositionRoutine());
    }

    private IEnumerator MovePositionRoutine()
    {   
        while (_cart.m_Position < _targetPosition)
        {
            float newPosition = Mathf.Lerp(_previousPosition, _targetPosition, _positionPercentage);
            _positionPercentage += _cameraDollySpeed;
            _cart.m_Position = newPosition;
            yield return null;
        }
        _positionPercentage = 0;
        _previousPosition = _targetPosition;

        _currentListPositionTracker++;
        if (_currentListPositionTracker >= _targetPositionsList.Count)
            yield return null;
        else
            _targetPosition = _targetPosition = _targetPositionsList[_currentListPositionTracker];

        _reachedTargetPositionEventChannel.RaiseEvent();
    }

    private IEnumerator LevelWalkthroughRoutine(CinemachineDollyCart cart)
    {
        while (cart.m_Position < _targetPositionsList[_targetPositionsList.Count - 1])
        {
            while (cart.m_Position < _targetPosition)
            {
                float newPosition = Mathf.Lerp(_previousPosition, _targetPosition, _positionPercentage);
                _positionPercentage += _cameraDollySpeed;
                _cart.m_Position = newPosition;
                yield return null;
            }
            _positionPercentage = 0;
            _previousPosition = _targetPosition;
            _currentListPositionTracker++;
            if (_currentListPositionTracker >= _targetPositionsList.Count)
                yield return null;
            else
                _targetPosition = _targetPosition = _targetPositionsList[_currentListPositionTracker];

            yield return null;
        }

        _playerWinEventChannel.RaiseEvent();
    }

    public int GetCurrentPosition()
    {
        return _currentListPositionTracker;
    }
}
