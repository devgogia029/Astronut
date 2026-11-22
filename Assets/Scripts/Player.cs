using UnityEngine;

public class Player : MonoBehaviour
{

    private CameraController cam;
    private Animator animator; //reference to the players animator component; private so can't be changed in inspector
    private SpriteRenderer spriteRenderer; //reference to the players sprite renderer component; private so can't be changed in inspector
    private Rigidbody2D platformRb; //reference to the moving platform physics component; private so can't be changed in inspector
    public bool isDead; //true when player is dead; false when alive; public so can be changed in the inspector
    public float FlipVelocityKeep = 0.3f; //how much of the players velocity is kept when flipping gravity; public so can be changed in the inspector
    public float moveSpeed = 7f; //how fast the player moves; public so can be changed in the inspector
    public Transform groundCheck; //empty object at players feet; drag it in via inspector
    public float groundRadius = 0.2f; //size of the invisible circle used to detect ground
    public LayerMask groundLayer; //layer that counts as ground; i set in the inspector to "Ground" layer
    Rigidbody2D rb; //reference to the players physics component; private so can't be changed in inspector
    public bool isGrounded; //true when player touching ground or ceiling; false when in air
    public bool flipLevelView; //switch ill tick level 3

    void Start()
    {
        cam = FindAnyObjectByType<CameraController>();
        rb = GetComponent<Rigidbody2D>(); //finds the Rigidbody2D on the player object and stores in rb
        animator = GetComponent<Animator>(); //finds the Animator on the player object and stores in animator
        spriteRenderer = GetComponent<SpriteRenderer>(); //finds the SpriteRenderer on the player object and stores in spriteRenderer
    }
    
    void Update() //runs every frame; input read here
    {
        if(Time.timeScale == 0f) //if the game is paused
            return; //skip the rest of the update method
        float x = isDead ? 0f : Input.GetAxisRaw("Horizontal");//+1 or -1 when flipped so D will always be right;
        animator.SetBool("isRunning", x != 0); //sets the isRunning bool in the animator to true if x is not 0; false if x is 0
        if (x > 0) //if x is greater than 0 (player moving right)
            spriteRenderer.flipX = false; //sets the players sprite to face right
        else if (x < 0) //if x is less than 0 (player moving left)
            spriteRenderer.flipX = true; //sets the players sprite to face left
        float platformSpeed = platformRb != null ? platformRb.linearVelocity.x : 0f; //gets the horizontal speed of the moving platform the player is standing on
        if (!isDead)
            rb.linearVelocity = new Vector2(x * moveSpeed + platformSpeed, rb.linearVelocity.y); //sets the players horizontal speed based on input; keeps vertical speed the same

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer); //checks if the player is touching the ground or ceiling; returns true or false
        if (Input.GetKeyDown(KeyCode.Space) && !isDead) //if spacebar pressed and player is not dead
            FlipGravity(); //calls the method to flip the players gravity
       
    }

    void FlipGravity() //flips the player gravity
    {
        AudioManager.Instance.PlayFlip(); //Plays gravity flip sfx
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * FlipVelocityKeep); //keeps some of the players vertical velocity when flipping gravity
        rb.gravityScale *= -1f; //flips the players gravity
        if (flipLevelView)
            cam.FlipView(rb.gravityScale < 0f);
        Vector3 s = transform.localScale; //gets the players current scale
        s.y *= -1f; //flips the players y scale
        transform.localScale = s; //sets the players scale to the new flipped scale
    }
    
    public void ResetFlip() //resets the players gravity and scale to normal; called by the PlayerRespawn when the player dies
    {
        rb.gravityScale = Mathf.Abs(rb.gravityScale); //sets the players gravity to normal
        if (flipLevelView)
            cam.FlipView(false);
        Vector3 s = transform.localScale; //gets the players current scale
        s.y = Mathf.Abs(s.y); //sets the players y scale to normal
        transform.localScale = s; //sets the players scale to the new normal scale
        rb.linearVelocity = Vector2.zero; //resets the players velocity to zero
        platformRb = null; //sets the platformRb to null so the player is no longer affected by the moving platform
    }
    
    void OnCollisionStay2D(Collision2D col) //runs every frame the player is touching a platform
    {
        if (col.gameObject.CompareTag("Platform")) //checks if the object the player is touching has the tag "MovingPlatform"
        {
            platformRb = col.rigidbody; //gets the Rigidbody2D of the moving platform and stores in platformRb
        }
    }
    
    void OnCollisionExit2D(Collision2D col) //runs when the player stops touching a platform
    {
        if (col.gameObject.CompareTag("Platform")) //checks if the object the player stopped touching is the platform
        {
            platformRb = null; //sets the platformRb to null so the player is no longer affected by the moving platform
        }
    }

    public void PlayFootstep()
    {
        if (isGrounded && !isDead)
        {
            AudioManager.Instance.PlayFootstep();
        }
    }
}
