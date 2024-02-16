using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUi;
    [SerializeField] private GameObject audioSettingsPanel;
    // Start is called before the first frame update
    void Start()
    {
        audioSettingsPanel.SetActive(false);
        pauseMenuUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuUi.activeSelf)
        {
            Resume();
        }
        else
        {
            Pause();
        }

    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
    }

    public void RestartGame()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowAudioSetting()
    {
        audioSettingsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    } 

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
