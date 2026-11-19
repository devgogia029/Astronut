using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsButtons : MonoBehaviour
{
    public void NextLevel()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        string nextName = "Level" + (GameData.currentLevel + 1); //builds the next scene's name
        if(Application.CanStreamedLevelBeLoaded(nextName)) //checks if the next scene exists
        {
            SceneManager.LoadScene(nextName); //loads the next scene
        }
        else
        {
            SceneManager.LoadScene("MainMenu"); //if the next scene doesn't exist, loads the main menu
        }
    }
    
    public void LoadMenu()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        SceneManager.LoadScene("MainMenu"); //loads the main menu
    }

    public void Restart()
    {
        AudioManager.Instance.PlayClick(); // plays sfx for button click
        AudioManager.Instance.RestartMusic();
        SceneManager.LoadScene("Level" + GameData.currentLevel); //reloads the current level
    }
}
