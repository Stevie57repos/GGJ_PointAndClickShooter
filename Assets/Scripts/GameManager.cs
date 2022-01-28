using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LevelCameraManager _levelCamManager;

    [Header("Event Channels")]
    [SerializeField]
    private PlayerWinEventSO _playerWinEventChannel;
    [SerializeField]
    private PlayerLoseEventChannel _playerLoseEventChannel;

    [SerializeField]
    private Transform _player;

    private void Start()
    {
        _levelCamManager.NextCameraPosition();
    }

    private void OnEnable()
    {
        _playerWinEventChannel.PlayerWinEvent += PlayerWin;
        _playerLoseEventChannel.PlayerLoseEvent += PlayerLose;
    }

    private void OnDisable()
    {
        _playerWinEventChannel.PlayerWinEvent -= PlayerWin;
        _playerLoseEventChannel.PlayerLoseEvent -= PlayerLose;
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
