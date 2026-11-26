using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float targetRotation; //angle the camera turns towards
    public float rotateSpeed = 6f; // speed at which the camera flips
    private Camera mainCamera; //reference to the main camera; private so can't be changed in inspector
    public float targetSize; //the size the camera will zoom to; public so can be changed in the inspector
    public Vector3 targetPosition; //the position the camera is moving towards; public so can be changed in the inspector
    public float smoothSpeed = 3f; //how fast the camera moves; public so can be changed in the inspector
    public Player player; //the player, so camera can copy his rotation and follow him
    private Vector3 homePosition; //where the camera sits when the level is upright
    private float baseSize; //normal zoom size
    public float sidewaysSize = 5.5f; //the zoom when the level is on its side
    public float roomHalfWidth = 17.78f; //half the rooms width, so the camera never shows outside it
    public float roomHalfHeight = 10f; //half the rooms height
    public float quarterFollowSpeed = 14f; //how tightly the camera follows him in this mode, higher number means less lag
    public float lookAhead = 0.08f; //secs ahead the camera aims, so leads a falling player instead of trailing him
    private Rigidbody2D playerRb; //players physics body, so we can read his speed

    void Start()
    {
        mainCamera = GetComponent<Camera>(); //gets the camera component on this object and stores it in mainCamera
        targetSize = mainCamera.orthographicSize; //sets the target size to the cameras current size
        targetPosition = transform.position; //sets the target position to the cameras current position
        player = FindAnyObjectByType<Player>(); //finds player in the scene
        if (player != null)
            playerRb = player.GetComponent<Rigidbody2D>(); //stores rigidbody only if player exists in the scene
        homePosition = transform.position; //remembers the upright camera position
        baseSize = mainCamera.orthographicSize; //remembers normal zoom
    }
    
    void LateUpdate() //runs after all other updates; used for camera movement so it doesn't jitter
    {
        if (player != null && player.quarterTurnMode)
        {
            FollowQuarterTurn(); //uses the special camera movement
            return; //skips the normal movement below
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); //moves the camera towards the target position smoothly
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, smoothSpeed * Time.deltaTime); //zooms the camera towards the target size smoothly
        float z = Mathf.LerpAngle(transform.eulerAngles.z, targetRotation, rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, z); //applies angle to camera
    }

    void FollowQuarterTurn()
    {
        float z = player.transform.eulerAngles.z; //players current angle
        transform.rotation = Quaternion.Euler(0f, 0f, z); //camera turns exactly with player
        float rad = z * Mathf.Deg2Rad; //angle in radians, for sin and cos
        float side = Mathf.Abs(Mathf.Sin(rad)); //0 when upright or upside down, 1 when on the sides
        float up = Mathf.Abs(Mathf.Cos(rad)); //1 when up or down, 0 when on the sides
        float aspect = mainCamera.aspect; //screen width divided by height
        float zoneHalfH = baseSize; //zomes half height
        float zoneHalfW = baseSize * aspect; //zones half width
        float sidewaysSize = zoneHalfH / aspect; //zoom that fits the zones height exactly into screen width
        float s = Mathf.Lerp(baseSize, sidewaysSize, side); //zoom for current angle
        mainCamera.orthographicSize = s; //applies it directly, angle already eases
        float halfX = up * aspect * s + side * s; //how far the view reaches along the worlds x axis
        float halfY = side * aspect * s + up * s; //how far the view reaches along the worlds y axis
        float rangeX = Mathf.Max(0f, zoneHalfW - halfX); //how far the camera may move in x and still stay inside the room
        float rangeY = Mathf.Max(0f, zoneHalfH - halfY); //how far the camera may move in y and still stay inside the room
        Vector3 target = player.transform.position + (Vector3)(playerRb.linearVelocity * lookAhead); //aims a little ahead of where player is
        float tx = Mathf.Clamp(target.x, homePosition.x - rangeX, homePosition.x + rangeX); //follows him in x inside room
        float ty = Mathf.Clamp(target.y, homePosition.y - rangeY, homePosition.y + rangeY); //follows him in y inside room
        float t = 1f - Mathf.Exp(-quarterFollowSpeed * Time.deltaTime); //a smoothing amount that behaves the same at any frame rate
        transform.position = Vector3.Lerp(transform.position, new Vector3(tx, ty, homePosition.z), t); //glides smoothly
    }
    
    public void MoveTo(Vector3 newTarget, float newSize) //called by other scripts to move the camera to a new position
    {
        targetPosition = newTarget; //sets the target position to the new position
        targetSize = newSize; //sets the target size to the new size
        homePosition = newTarget; //keeps home position in step
        baseSize = newSize; //keeps normal zoom in step
    }

    public void FlipView(bool flipped)
    {
        targetRotation = flipped ? 180f : 0f;
    }
}
