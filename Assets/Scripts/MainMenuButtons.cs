using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject exitButton;

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            exitButton.SetActive(false);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        SceneManager.LoadScene("Level1"); //loads the first level
    }

    public void OpenCredits()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        SceneManager.LoadScene("Credits"); //loads the credits scene
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        Debug.Log("Quit pressed"); //logs a message to the console for debugging purposes
        Application.Quit(); //quits the application
    }
}
