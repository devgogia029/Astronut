using UnityEngine;

public static class GameData
{
    public static int currentLevel = 1; //the current level the player is on; static so it can be accessed from anywhere
    public static float lastTime; //the time in sconds the player took to complete the level
    public static int diamondsCollected; //the number of diamonds the player collected in the level
    public static int diamondsTotal; //the total number of diamonds in the level
    public static float bestTime; //the best time saved for the level
    public static bool newBest; //true when the level just finished beat an older best time
}
