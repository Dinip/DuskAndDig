using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField]
    //private GameManagerObject gm;

    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject settingsMenu;

    private void Start()
    {
        Cursor.visible = true;
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        SceneManager.LoadScene("Action2D");
    }

    public void ShowSettings()
    {
        startMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }


    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    public void ShowInitialMenu()
    {
        startMenu.SetActive(true);
        //difficultyMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            ShowInitialMenu();
        }
    }
}