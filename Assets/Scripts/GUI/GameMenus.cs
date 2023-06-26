using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenus : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private GameManagerObject gameManager;

    [SerializeField]
    private InventoryObject playerEquipment;

    [SerializeField]
    private TimeControllerObject timeControllerObj;

    [SerializeField]
    private EnemyObject enemyObject;

    [SerializeField]
    private BuildingsSet buildings;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject loseMenuUI;

    [SerializeField]
    private GameObject confirmMenu;

    private string _pendingAction;

    private string _pendingActionCaller;

    private void OnEnable()
    {
        eventBus.gameOverEvent.AddListener(ShowGameOver);
        var vol = PlayerPrefs.GetFloat("volume", .8f);
        AudioListener.volume = vol;
    }

    private void OnDisable()
    {
        eventBus.gameOverEvent.RemoveListener(ShowGameOver);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager.IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        gameManager.SetPause(false);
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        gameManager.SetPause(true);
    }

    public void LoadMainMenu()
    {
        gameManager.ResetGame();
        StartCoroutine(WaitChangeScene("MainMenu", 0.5f));
    }

    public void ResetGame()
    {
        gameManager.ResetGame();
        StartCoroutine(WaitChangeScene(SceneManager.GetActiveScene().name, 0.5f));
    }

    private IEnumerator WaitChangeScene(string scene, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        loseMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameManager.SetPause(false);
        SceneManager.LoadScene(scene);
    }

    private void ShowGameOver(bool _)
    {
        loseMenuUI.SetActive(true);
        gameManager.SetPause(true);

        TextMeshProUGUI statsTextUI = GameObject.Find("StatsText").GetComponent<TextMeshProUGUI>();
        statsTextUI.SetText(ReplaceText(statsTextUI.text));
    }

    private string ReplaceText(string text)
    {
        Dictionary<string, string> replace = new()
        {
            { "{DAY}", timeControllerObj.currentDay.ToString() },
            { "{EHP}", Mathf.RoundToInt(enemyObject.Health).ToString()},
            { "{EDMG}", Mathf.RoundToInt(enemyObject.Damage).ToString() },
            { "{PSH}", Mathf.RoundToInt(Utils.ComputePlayerShield(playerEquipment)).ToString() },
            { "{PDMG}", Mathf.RoundToInt(Utils.ComputePlayerDamage(playerEquipment, buildings)).ToString() }
        };

        foreach (KeyValuePair<string, string> r in replace)
        {
            text = text.Replace(r.Key, r.Value);
        }

        return text;
    }

    public void ConfirmActionName(string action)
    {
        _pendingAction = action;
        if (pauseMenuUI.activeSelf)
        {
            _pendingActionCaller = "pauseMenuUI";
            pauseMenuUI.SetActive(false);
        }
        else if (loseMenuUI.activeSelf)
        {
            _pendingActionCaller = "loseMenuUI";
            loseMenuUI.SetActive(false);
        }
        confirmMenu.SetActive(true);
    }

    public void ConfirmAction(bool value)
    {
        if (value)
        {
            if (_pendingAction == "") return;
            MethodInfo method = GetType().GetMethod(_pendingAction);
            _pendingAction = "";
            method.Invoke(this, null);
        }
        else
        {
            if (_pendingActionCaller == "") return;
            if (_pendingActionCaller == "pauseMenuUI")
            {
                pauseMenuUI.SetActive(true);
            }
            else if (_pendingActionCaller == "loseMenuUI")
            {
                loseMenuUI.SetActive(true);
            }
            _pendingActionCaller = "";
            confirmMenu.SetActive(false);
        }
    }
}
