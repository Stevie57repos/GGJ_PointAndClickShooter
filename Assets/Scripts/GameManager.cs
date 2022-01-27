using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerWinEventSO _playerWinEventChannel;

    [SerializeField]
    private PlayerLoseEventChannel _playerLoseEventChannel;

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
}
