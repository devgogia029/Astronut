using UnityEngine;

public class HintZone : MonoBehaviour
{
    public SpeechBubble bubble; //the bubble to use; drag it in
    [TextArea] public string message; //what it says; press Enter for a second line
    public DismissOn dismissOn = DismissOn.Timer; //what makes it disappear: Timer, Move or Flip
    public float showSeconds = 5f; //the most it stays on screen
    private bool used; //true after it has shown once, so it never nags
    void OnTriggerEnter2D(Collider2D other) //runs when something enters the zone
    {
        if (used || !other.CompareTag("Player")) return; //ignores everything except the player, and only works once
        used = true; //marks it as shown
        if (bubble == null)
            bubble = FindAnyObjectByType<SpeechBubble>(); //finds the scenes bubble if the slot was left empty
        bubble.Show(message, dismissOn, showSeconds); //shows the hint
    }
}