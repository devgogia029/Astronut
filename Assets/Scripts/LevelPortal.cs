using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private LevelManager manager; //reference to the level manager script
    private SpriteRenderer spriteRenderer; //reference to the sprite renderer component
    private Animator animator; //reference to the animator component
    public Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 1f); //color of the portal when locked
    private bool isOpen; //true if the portal is open; false if locked
    
    void Start()
    {
        manager = FindAnyObjectByType<LevelManager>(); //finds the LevelManager script in the scene and stores it in manager
        spriteRenderer = GetComponent<SpriteRenderer>(); //finds the SpriteRenderer component on this object and stores it in spriteRenderer
        animator = GetComponent<Animator>(); //finds the Animator component on this object and stores it in animator
        spriteRenderer.color = lockedColor; //dims the color
        animator.speed = 0f; //freezes the animation
    }

    void Update()
    {
        if (!isOpen && manager.AllCollected()) //if the portal is locked and all diamonds are collected
        {
            OpenPortal(); //opens the portal as soon as all diamonds are collected
        }
    }

    void OpenPortal() //opens the portal
    {
        AudioManager.Instance.PlayPortalOpen(); //plays sfx of portal opening
        isOpen = true; //sets the portal to open
        spriteRenderer.color = Color.white; //resets the color to normal
        animator.speed = 1f; //unfreezes the animation
    }

    void OnTriggerEnter2D(Collider2D other) //called when player enters the trigger
    {
        if (isOpen && other.CompareTag("Player")) //if the portal is open and the player enters
        {
            manager.CompleteLevel();
        }
        else if (!isOpen && other.CompareTag("Player")) //if the portal is locked and the player enters
        {
            Debug.Log("Collect all diamonds to unlock the portal!"); //logs a message to the console
        }
    }
}
