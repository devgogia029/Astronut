using UnityEngine;
using TMPro;

public enum DismissOn { Timer, Move, Flip}
public class SpeechBubble : MonoBehaviour
{

    public GameObject visual; //the box and text together, so one switch hides both
    public SpriteRenderer background; //the bubble's box
    public TextMeshPro label; //the text inside it
    public float heightOffset = 1.1f; //how far above his head it floats, in world units
    public Vector2 padding = new Vector2(0.5f, 0.35f); //space between the text and the box edge
    public float moveSeconds = 1.5f; //how long he must walk before a Move hint is considered learned
    private Transform target; //who it follows
    private Rigidbody2D targetRb; //his physics body, to read which way is "up" for him
    private bool showing; //true while a hint is on screen
    private float timer; //seconds left before the hint disappears by itself
    private float moved; //seconds he has spent walking since the hint appeared
    private DismissOn mode; //what makes the current hint disappear
    void Start()
    {
        Player p = FindAnyObjectByType<Player>(); //finds the player in this scene
        target = p.transform; //remembers who to follow
        targetRb = p.GetComponent<Rigidbody2D>(); //remembers his physics body
        visual.SetActive(false); //starts hidden
    }


    void Update()
    {
        if (!showing) return; //nothing to do while hidden
        if (Time.timeScale == 0f) return; //don't count down while paused
        timer -= Time.deltaTime; //counts the time down
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f) moved += Time.deltaTime; //adds up the time he spends walking
        bool flipped = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift); //true on the frame any flip or turn key is pressed
        if (timer <= 0f) Hide(); //time is up
        else if (mode == DismissOn.Flip && flipped) Hide(); //he flipped, so the hint did its job
        else if (mode == DismissOn.Move && moved >= moveSeconds) Hide(); //he has walked enough to have learned it
    }

    void LateUpdate()
    {
        Vector2 up = -(Physics2D.gravity * Mathf.Sign(targetRb.gravityScale)).normalized; //the direction from his feet to his head, even when flipped or turned
        transform.position = (Vector2)target.position + up * heightOffset; //floats above his head
        transform.rotation = Camera.main.transform.rotation; //stays upright on screen even when the level is turned
        if (Camera.main.transform.eulerAngles.z != 0f) return; //skip this in the turned 90 degree mode, where the maths is different
        float halfW = Camera.main.orthographicSize * Camera.main.aspect - background.size.x * 0.5f; //how far the box centre may go from the camera centre before an edge leaves the screen
        float cx = Camera.main.transform.position.x; //the camera's centre
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, cx - halfW, cx + halfW), transform.position.y, transform.position.z); //slides the box back inside the screen
    }

    public void Show(string message, DismissOn how, float seconds)
    {
        visual.SetActive(true); //shows it first
        label.text = message; //puts the message in
        label.ForceMeshUpdate(); //makes TextMeshPro measure the text right now
        Vector2 box = (Vector2)label.textBounds.size + padding * 2f; //text size plus padding on every side
        background.size = box; //grows the box to fit the text
        mode = how; //remembers how it should disappear
        timer = seconds; //starts the countdown
        moved = 0f; //resets the walking time
        showing = true; //marks the hint as active
        visual.SetActive(true); //shows it
    }

    void Hide()
    {
        showing = false; visual.SetActive(false); //hides the hint and stops the timer
    }
}
