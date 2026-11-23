using UnityEngine;

public class MenuBob : MonoBehaviour
{

    private Vector3 baseScale;
    public float bobHeight = 0.2f; //how far up and down it moves
    public float bobSpeed = 2f; //how fast it bobs
    private Vector3 startPos; //where the diamond started, so it bobs around this spot
    void Start()
    {
        startPos = transform.position; //remembers where the diamond was placed
        baseScale = transform.localScale;
    }


    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobHeight; //moves it up and down smoothly
        transform.localScale = baseScale * (1f + Mathf.Sin(Time.time * 3f) * 0.05f);
    }
}
