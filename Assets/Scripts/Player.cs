using UnityEngine;

public class Player : MonoBehaviour
{

    private CameraController cam;
    private Animator animator; //reference to the players animator component; private so can't be changed in inspector
    private SpriteRenderer spriteRenderer; //reference to the players sprite renderer component; private so can't be changed in inspector
    private Rigidbody2D platformRb; //reference to the moving platform physics component; private so can't be changed in inspector
    private int gravityAngle; //0 = gravity down, 90 = right, 180 = up, 270 = left
    private Vector2 normalGravity = new Vector2(0f, -9.81f); //usual world gravity, so it can be restored

    public float turnSpeed = 540f; //degrees per second the astronaut turns onto his new floor
    public bool isDead; //true when player is dead; false when alive; public so can be changed in the inspector
    public float FlipVelocityKeep = 0.3f; //how much of the players velocity is kept when flipping gravity; public so can be changed in the inspector
    public float moveSpeed = 7f; //how fast the player moves; public so can be changed in the inspector
    public Transform groundCheck; //empty object at players feet; drag it in via inspector
    public float groundRadius = 0.2f; //size of the invisible circle used to detect ground
    public LayerMask groundLayer; //layer that counts as ground; i set in the inspector to "Ground" layer
    Rigidbody2D rb; //reference to the players physics component; private so can't be changed in inspector
    public bool isGrounded; //true when player touching ground or ceiling; false when in air
    public bool flipLevelView; //switch will tick level 3
    public bool quarterTurnMode; //when ticked, Q and E turn gravity 90 degree; when off, space will flip 180


    void Start()
    {
        cam = FindAnyObjectByType<CameraController>();
        rb = GetComponent<Rigidbody2D>(); //finds the Rigidbody2D on the player object and stores in rb
        animator = GetComponent<Animator>(); //finds the Animator on the player object and stores in animator
        spriteRenderer = GetComponent<SpriteRenderer>(); //finds the SpriteRenderer on the player object and stores in spriteRenderer
        if (quarterTurnMode)
            Physics2D.gravity = normalGravity; //makes sure a new scene starts with normal Gravity
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
        if (quarterTurnMode)
            MoveAlongFloor(x); //moves along whichever surface he is on
        else if (!isDead)
            rb.linearVelocity = new Vector2(x * moveSpeed + platformSpeed, rb.linearVelocity.y); //sets the players horizontal speed based on input; keeps vertical speed the same

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer); //checks if the player is touching the ground or ceiling; returns true or false
        if (quarterTurnMode)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isDead)
                TurnGravity(1); //Space turns the level right
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDead)
                TurnGravity(-1); //Either shift turns level left
            float z = Mathf.MoveTowardsAngle(transform.eulerAngles.z, gravityAngle, turnSpeed * Time.deltaTime); //turns him step by step onto his new fllor
            transform.rotation = Quaternion.Euler(0f, 0f, z); //applies the turn
        }
        
        else if (Input.GetKeyDown(KeyCode.Space) && !isDead) //if spacebar pressed and player is not dead
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
        if (quarterTurnMode)
        {
            gravityAngle = 0;
            Physics2D.gravity = normalGravity;
            transform.rotation = Quaternion.identity; //gravity down, upright
        }
        rb.linearVelocity = Vector2.zero; //resets the players velocity to zero
        platformRb = null; //sets the platformRb to null so the player is no longer affected by the moving platform
    }

    Vector2 GravityDir()
    {
        return Quaternion.Euler(0f, 0f, gravityAngle) * Vector2.down; //a unit vector pointing the way gravity pulls
    }

    Vector2 RightDir()
    {
        Vector2 d = GravityDir();
        return new Vector2(-d.y, d.x); //his "right", along the surface he stands on
    }

    void OnDestroy()
    {
        Physics2D.gravity = normalGravity; //puts grav back to normal when player is removed, so it cannot leak into the next scene
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

    void TurnGravity(int dir) //turns grav 90 degrees; dir is +1 for right (E) and -1 for left (Q)
    {
        AudioManager.Instance.PlayFlip(); //pplays the flip sound
        Vector2 oldDown = GravityDir(); //the way grav pulled before the turn
        float fall = Vector2.Dot(rb.linearVelocity, oldDown); //how fast he was falling that way
        rb.linearVelocity -= oldDown * fall * (1f - FlipVelocityKeep); //keeps only a fraction of the fall speed, like old flip
        gravityAngle = (gravityAngle + dir * 90 + 360) % 360; //turns the angle and wraps it into 0 to 359
        Physics2D.gravity = GravityDir() * normalGravity.magnitude; //points the worlds gravity the new way
    }

    void MoveAlongFloor(float x)
    {
        if (isDead)
            return; //no control when dead
        Vector2 right = RightDir(); //his right dir right now
        float platformAlong = platformRb != null ? Vector2.Dot(platformRb.linearVelocity, right) : 0f; //the moving platforms speed along that direction
        Vector2 v = rb.linearVelocity; //his current velocity
        Vector2 fallPart = v - right * Vector2.Dot(v, right); //the part ont along the floor(Falling or rising)
        rb.linearVelocity = right * (x * moveSpeed + platformAlong) + fallPart; //sets the speed along the floor and keeps the falling part
    }
}
