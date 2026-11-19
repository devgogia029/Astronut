using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; //how fast the platform moves; public so can be changed in the inspector
    public float range = 4f; //how far the platform moves from its starting position; public so can be changed in the inspector
    private Rigidbody2D rb; //reference to the platforms physics component; private so can't be changed in inspector
    private Vector2 startPos; //where the object began
    public Vector2 direction = Vector2.right; //the direction it moves first, public, so per object can be set in inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //finds the Rigidbody2D on the platform object and stores in rb
        startPos = transform.position; //stores the starting x position of the platform
        direction = direction.normalized; //shrinks direction to 1, and speed behaves whaterver speed says
        rb.linearVelocity = direction * speed; //sets the platforms horizontal speed

        
    }

    void FixedUpdate() //runs every physics update; used for physics calculations
    {
        float travelled = Vector2.Dot((Vector2)transform.position - startPos, direction); //measures how far object has moved, + when ahead its direction and - when behind
        if (travelled >= range) //if the platform has moved too far to the right
        {
            rb.linearVelocity = -direction * speed; //sets the platforms horizontal speed to move left
        }
        else if (travelled <= -range) //if the platform has moved too far to the left
        {
            rb.linearVelocity = direction * speed; //sets the platforms horizontal speed to move right
        }
    }
}
