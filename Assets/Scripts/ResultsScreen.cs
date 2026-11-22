using UnityEngine;
using TMPro;

public class ResultsScreen : MonoBehaviour
{
    public TextMeshProUGUI diamondsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nextButtonText; //the label on the next level button
    
    void Start()
    {
        float t = GameData.lastTime; //gets the last time from the GameData script
        int minutes = (int)(t / 60f); //calculates the time
        timeText.text = "TIME " + string.Format("{0:00}:{1:00.00}", minutes, t % 60f); //displays the time in the format MM:SS.ss
        if (GameData.diamondsCollected >= GameData.diamondsTotal) //checks if the player collected all the diamonds
            diamondsText.text = "ALL DIAMONDS COLLECTED!"; //displays a message if all diamonds collected
        else //if not all diamonds collected
            diamondsText.text = GameData.diamondsCollected + " / " + GameData.diamondsTotal + " DIAMONDS"; //displays the number of diamonds collected and total diamonds
        string nextName = "Level" + (GameData.currentLevel + 1); //builds the name of the level that comes next
        if (Application.CanStreamedLevelBeLoaded(nextName)) //checks if that scene is in the build list
            nextButtonText.text = "NEXT LEVEL"; //another lvl exists, so the normal label
        else
            nextButtonText.text = "CONTINUE"; //this was the last level 
    }
}
