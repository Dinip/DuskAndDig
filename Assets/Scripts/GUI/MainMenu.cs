using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameManagerObject gm;

    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject howToPlayMenu;

    [SerializeField]
    private GameObject[] texts;

    [SerializeField]
    private GameObject[] buttons; // 0 = prev, 1 = next

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI sliderText;

    private int _currentTextIdx = 0;

    void Awake()
    {
#if !UNITY_EDITOR
        StartCoroutine("MoveToPrimaryDisplay");
#endif
    }

    private void OnEnable()
    {
        var vol = PlayerPrefs.GetFloat("volume", .8f);
        sliderText.text = $"Volume: {vol*100:0.00}%";
        slider.value = vol*100;
        slider.onValueChanged.AddListener(HandleVolumeSlider);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(HandleVolumeSlider);
    }

    private IEnumerable MoveToPrimaryDisplay()
    {
        List<DisplayInfo> displays = new();
        Screen.GetDisplayLayout(displays);
        if (displays?.Count > 0)
        {
            var moveOperation = Screen.MoveMainWindowTo(displays[0], new Vector2Int(displays[0].width / 2, displays[0].height / 2));
            yield return moveOperation;
        }
    }

    public void StartGame()
    {
        gm.ResetGame();
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
        howToPlayMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void ShowHelpMenu()
    {
        foreach (var t in texts)
        {
            t.SetActive(false);
        }
        startMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
        _currentTextIdx = 0;
        texts[_currentTextIdx].SetActive(true);
        buttons[0].SetActive(false);
        buttons[1].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowInitialMenu();
        }
    }

    public void Next()
    {
        texts[_currentTextIdx].SetActive(false);
        _currentTextIdx++;
        texts[_currentTextIdx].SetActive(true);
        if (_currentTextIdx == texts.Length - 1)
        {
            buttons[1].SetActive(false);
        }
        buttons[0].SetActive(true);
    }

    public void Previous()
    {
        texts[_currentTextIdx].SetActive(false);
        _currentTextIdx--;
        texts[_currentTextIdx].SetActive(true);
        if (_currentTextIdx == 0)
        {
            buttons[0].SetActive(false);
        }
        buttons[1].SetActive(true);
    }

    public void HandleVolumeSlider(float volume)
    {
        sliderText.text = $"Volume: {volume:0.00}%";
        PlayerPrefs.SetFloat("volume", volume/100);
    }
}