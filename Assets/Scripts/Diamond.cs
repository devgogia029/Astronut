using UnityEngine;

public class Diamond : MonoBehaviour
{
    private LevelManager manager; //reference to the level manager script; private so can't be changed in inspector

    void Start()
    {
        manager = FindAnyObjectByType<LevelManager>(); //finds the LevelManager script in the scene and stores it in manager
    }
    void OnTriggerEnter2D(Collider2D other) //called when another collider enters this trigger
    {
        if (other.CompareTag("Player")) //if the other collider is the player
        {
            manager.CollectDiamond(); //calls the CollectDiamond method in the LevelManager script
            AudioManager.Instance.PlayPickup(); // plays the sfx for diamond pickup
            Destroy(gameObject); //destroys the diamond object
        }
    }
}
