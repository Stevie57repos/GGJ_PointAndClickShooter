using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LevelCameraManager _levelCamManager;

    [SerializeField]
    private UnitSpawnManager _unitSpawnManager;

    [Header("Event Channels")]
    [SerializeField]
    private PlayerWinEventSO _playerWinEventChannel;
    [SerializeField]
    private PlayerLoseEventChannel _playerLoseEventChannel;
    [SerializeField]
    private ClearedEnemyWaveEventSO _clearedEnemyWaveEventChannel;
    [SerializeField]
    private ReachedTargetPositionEventSO _reachedTargetPositionEventChannel;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private bool isEnemyWaveCleared = false;
    [SerializeField]
    private bool hasReachedTargetPosition = false;

    private void Awake()
    {
        _unitSpawnManager.Setup(_player);
    }

    private void Start()
    {
        _unitSpawnManager.SpawnUnits(_levelCamManager.GetCurrentPosition());
        _levelCamManager.NextCameraPosition();
    }

    private void OnEnable()
    {
        _playerWinEventChannel.PlayerWinEvent += PlayerWin;
        _playerLoseEventChannel.PlayerLoseEvent += PlayerLose;
        _clearedEnemyWaveEventChannel.ClearedEnemyWaveEvent += ClearedEnemies;
        _reachedTargetPositionEventChannel.ReachedTargetPosition += AtTargetPosition;
    }

    private void OnDisable()
    {
        _playerWinEventChannel.PlayerWinEvent -= PlayerWin;
        _playerLoseEventChannel.PlayerLoseEvent -= PlayerLose;
        _clearedEnemyWaveEventChannel.ClearedEnemyWaveEvent -= ClearedEnemies;
        _reachedTargetPositionEventChannel.ReachedTargetPosition -= AtTargetPosition;
    }
    private void AtTargetPosition()
    {
        hasReachedTargetPosition = true;
        MoveLevelForward();
    }

    private void ClearedEnemies()
    {
        isEnemyWaveCleared = true;
        MoveLevelForward();
    }

    private void MoveLevelForward()
    {
        
        if(hasReachedTargetPosition && isEnemyWaveCleared)
        {
            print($"movve level forward called");
            isEnemyWaveCleared = false;
            hasReachedTargetPosition = false;
            _levelCamManager.NextCameraPosition();
            _unitSpawnManager.SpawnUnits(_levelCamManager.GetCurrentPosition());
        }
    }


    #region Debug Context Menu Test Methods

    [ContextMenu("Lose Now")]
    public void PlayerLose()
    {
        SceneManager.LoadSceneAsync("Lose", LoadSceneMode.Additive);
    }

    [ContextMenu("Win Now")]
    public void PlayerWin()
    {
        SceneManager.LoadScene("Win", LoadSceneMode.Additive);
    }
    #endregion
}
