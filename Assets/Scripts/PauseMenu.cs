using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pausePanel; //reference to the pause panel
    private bool isPaused; //flag to track if the game is paused

    void Start()
    {
        Time.timeScale = 1f; //ensures the game starts unpaused
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //checks if the Escape key is pressed
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) //checks if the R key is pressed
        {
            Restart();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true); //activates the pause panel
        Time.timeScale = 0f; //pauses the game by setting time scale to 0
        isPaused = true; //sets the paused flag to true
    }

    public void Resume()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        pausePanel.SetActive(false); //deactivates the pause panel
        Time.timeScale = 1f; //resumes the game by setting time scale to 1
        isPaused = false; //sets the paused flag to false
    }

    public void Restart()
    {
        Time.timeScale = 1f; //resumes the game by setting time scale to 1
        AudioManager.Instance.RestartMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reloads the current scene
    }

    public void LoadMenu()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        Time.timeScale = 1f; //resumes the game by setting time scale to 1
        SceneManager.LoadScene("MainMenu"); //loads the menu scene
    }
}
