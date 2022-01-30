using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuGO;
    [SerializeField]
    private GameObject _controlsGO;
    [SerializeField]
    private GameObject _creditsGO;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _menuClip;
    [SerializeField]
    private StopSoundEventChannelSO _stopSoundEventChannel;

    private void Awake()
    {
        Cursor.visible = true;
    }

    public void Start()
    {
        _stopSoundEventChannel.RaiseEvent();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenControlsUI()
    {
        _controlsGO.SetActive(true);
        _mainMenuGO.SetActive(false);
    }

    public void CloseControlsUI()
    {
        _controlsGO.SetActive(false);
        _mainMenuGO.SetActive(true);
    }

    public void OpenCreditsUI()
    {
        _creditsGO.SetActive(true);
        _mainMenuGO.SetActive(false);
    }

    public void CloseCreditsUI()
    {
        _creditsGO.SetActive(false);
        _mainMenuGO.SetActive(true);
    }

    //public void QuitGame()
    //{
    //    Application.Quit();
    //}
}
