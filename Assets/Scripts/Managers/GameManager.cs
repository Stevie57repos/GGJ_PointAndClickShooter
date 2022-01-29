using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{  
    [SerializeField]
    private int _StartAtShootout;
    [SerializeField]
    private Transform _player;
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
    private LevelEnemiesCleaeredEventSO _levelEnemiesClearedEvent;
    private bool _levelEnemiesCleared = false;
    [SerializeField]
    private LevelEndEventSO _levelEndEventChannel;

    [Header("Debugging")]
    [SerializeField]
    private bool _isSpectateLevel; 
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
        if (_isSpectateLevel) _levelCamManager.SpectateLevel();
        else
        {
            //_unitSpawnManager.SpawnUnits(_levelCamManager.GetCurrentPosition());
            _levelCamManager.StartAtShootOut(_StartAtShootout);
        }
    }

    private void OnEnable()
    {
        _playerWinEventChannel.PlayerWinEvent += PlayerWin;
        _playerLoseEventChannel.PlayerLoseEvent += PlayerLose;
        _clearedEnemyWaveEventChannel.ClearedEnemyWaveEvent += ClearedEnemies;
        _reachedTargetPositionEventChannel.ReachedTargetPosition += AtTargetPosition;
        _levelEnemiesClearedEvent.ClearedlevelEnemiesEvent += AllEnemiesDead;
        _levelEndEventChannel.LevelEndEvent += PlayerWin;
    }

    private void OnDisable()
    {
        _playerWinEventChannel.PlayerWinEvent -= PlayerWin;
        _playerLoseEventChannel.PlayerLoseEvent -= PlayerLose;
        _clearedEnemyWaveEventChannel.ClearedEnemyWaveEvent -= ClearedEnemies;
        _reachedTargetPositionEventChannel.ReachedTargetPosition -= AtTargetPosition;
        _levelEnemiesClearedEvent.ClearedlevelEnemiesEvent -= AllEnemiesDead;
        _levelEndEventChannel.LevelEndEvent -= PlayerWin;
    }

    private void AllEnemiesDead()
    {
        _levelEnemiesCleared = true;
        GameWonCheck();
    }

    private void GameWonCheck()
    {
        if (_levelEnemiesCleared)
            PlayerWin();
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
            isEnemyWaveCleared = false;
            hasReachedTargetPosition = false;
            _levelCamManager.NextCameraPosition();
            _unitSpawnManager.SpawnUnits(_levelCamManager.GetCurrentPosition());
        }
    }

    [ContextMenu("Lose Now")]
    public void PlayerLose()
    {
        StopGame();
        SceneManager.LoadSceneAsync("Lose", LoadSceneMode.Additive);
    }

    [ContextMenu("Win Now")]
    public void PlayerWin()
    {
        StopGame();
        SceneManager.LoadScene("Win", LoadSceneMode.Additive);
    }

    private void StopGame()
    {
        _levelCamManager.Stop();
        _unitSpawnManager.Stop();
    }

}
