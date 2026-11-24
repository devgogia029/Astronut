using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int levelNumber = 1; //the level number, set in inspector
    public float levelTime; //the time in seconds the player took to complete the level
    private bool timerRunning = true; //true while the clock counts
    public TextMeshProUGUI timerText; //the on-screen timer
    public TextMeshProUGUI diamondText; //reference to the diamond text object in the UI
    public int totalDiamonds; //total number of diamonds in the level, counted automatically
    public int collectedDiamonds; //number of diamonds collected by the player

    void Start()
    {
        totalDiamonds = GameObject.FindGameObjectsWithTag("Diamond").Length; //finds all objects with the tag "Diamond" and counts them
        UpdateText(); //updates the diamond text in the UI
    }
    public void CollectDiamond() //called by the Diamond script when the player collects a diamond
    {
        collectedDiamonds++; //adds 1 to the number of diamonds collected
        UpdateText(); //updates the diamond text in the UI
    }
    public bool AllCollected() //returns true if all diamonds collected; false if not
    {
        return collectedDiamonds >= totalDiamonds; //checks if the number of diamonds collected is greater than or equal to the total number of diamonds
    }
    void UpdateText() //updates the diamond text in the UI
    {
        diamondText.text = collectedDiamonds + " / " + totalDiamonds; //sets the text to show the number of diamonds collected and the total number of diamonds
    }
    void Update()
    {
        if(timerRunning)
        {
            levelTime += Time.deltaTime;
        }
        int minutes = (int)(levelTime / 60f);
        timerText.text = string.Format("{0:00}:{1:00.00}", minutes, levelTime % 60f); //formats the time as MM:SS.ss and updates the timer text in the UI
    }
    public void CompleteLevel()
    {
        timerRunning = false;
        GameData.currentLevel = levelNumber;
        GameData.lastTime = levelTime;
        GameData.diamondsCollected = collectedDiamonds;
        GameData.diamondsTotal = totalDiamonds;
        AudioManager.Instance.PlayLevelComplete(); //plays sfx for completing the level
        string key = "Best Time_" + levelNumber; //the saved-data name for this level
        float oldBest = PlayerPrefs.GetFloat(key, -1f); //loads the saved best time; -1 means none saved yet
        GameData.newBest = oldBest > 0f && levelTime < oldBest; //true only when an existing best is broken
        if (oldBest < 0f || levelTime < oldBest) //first finish, or faster than the old best
        {
            PlayerPrefs.SetFloat(key, levelTime); //saves the new best
            PlayerPrefs.Save(); //writes it to storage right away, which WebGL needs
        }
        GameData.bestTime = PlayerPrefs.GetFloat(key); //passes the best time to the results screen
        SceneManager.LoadScene("LevelComplete");
    }
}
