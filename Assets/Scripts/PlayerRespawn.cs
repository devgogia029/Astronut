using UnityEngine;
using System.Collections; //allows use of coroutines

public class PlayerRespawn : MonoBehaviour
{
    public float kickBackX = 4f; //how much the player is pushed back when hit by a hazard; public so can be changed in the inspector
    public float kickBackY = 6f; //how much the player is pushed up when hit by a hazard; public so can be changed in the inspector
    public float deathDelay = 1f; //how long to wait before respawning the player; public so can be changed in the inspector
    private Rigidbody2D rb; //players physics components
    private Animator animator; //players animator component
    private SpriteRenderer spriteRenderer; //used to see which way the player is facing
    private Vector3 respawnPosition; //stores the position the player will respawn at; private so can't be changed in inspector
    private Player player; //reference to the Player script; private so can't be changed in inspector

    void Start()
    {
        respawnPosition = transform.position; //sets the respawn position to the players starting position
        player = GetComponent<Player>(); //finds the Player script on the player object and stores in player
        rb = GetComponent<Rigidbody2D>(); //finds the Rigidbody2D on the player object and stores in rb
        animator = GetComponent<Animator>(); //finds the Animator on the player object and stores in animator
        spriteRenderer = GetComponent<SpriteRenderer>(); //finds the SpriteRenderer on the player object and stores in spriteRenderer

    }
    void OnTriggerEnter2D(Collider2D other) //runs when the player collides with a trigger collider
    {
        if (other.CompareTag("Hazard") && !player.isDead) //checks if the object the player collided with has the tag "Hazard"
        {
            StartCoroutine(DeathSequence()); //starts the DeathSequence coroutine
        }
    }

    void Respawn() //resets the players position to the respawn position; called by the Player touches a hazard
    {
        player.ResetFlip(); //resets the players gravity and scale to normal
        transform.position = respawnPosition; //moves the player to the respawn position
        player.isDead = false; //unlocks the controls
        animator.SetBool("isDead", false); //leaves the death animation state and returns to idle
    }

    public void SetCheckpoint(Vector3 newPosition) //sets the respawn position to a new position
    {
        respawnPosition = newPosition; //sets the respawn position to the new position
    }
    IEnumerator DeathSequence() //a method that can pause, used for the whole death process
    {
        float gravitySign = Mathf.Sign(rb.gravityScale); //+1 if gravity is normal, -1 if the player was flipped; read before resetting
        player.isDead = true; //locks the controls in the Player Script
        player.ResetFlip(); //puts the gravity and the sprte back to normal so he falls down
        float KickX = spriteRenderer.flipX ? kickBackX : -kickBackX; //if the player is facing left, push him right; if facing right, push him left
        rb.linearVelocity = new Vector2(KickX, gravitySign * kickBackY); //applies the kickback: up after hitting floor, down after hitting ceiling
        animator.SetBool("isDead", true); //enters the death animation state
        AudioManager.Instance.PlayDeath(); //plays sfc for death
        yield return new WaitForSeconds(0.15f); //pause briefly so he lifts off the surface
        float timer = 0f; //safety timer so the loop can never hang forever
        while (!player.isGrounded && timer < 2f) //keep waiting whiile he is in air, for at most 2 seconds
        {
            timer += Time.deltaTime; //adds the time since last frame to the timer
            yield return null; //waits until next frame to continue
        }
        rb.linearVelocity = Vector2.zero; //stops the player from moving
        yield return new WaitForSeconds(deathDelay); //waits for the death delay time
        Respawn(); //move him back to checkpoint
    }
}
