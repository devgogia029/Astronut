using UnityEngine;

[ExecuteAlways]
public class TileHazard : MonoBehaviour
{

    private SpriteRenderer sr;
    private BoxCollider2D box;
    public float shrinkTop = 0.25f; //this size shorter than the spike art

    void Start()
    {
        
    }

    void Update()
    {
        if (Application.isPlaying)
            return;
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        if (box == null)
            box = GetComponent<BoxCollider2D>();
        box.size = new Vector2(sr.size.x, sr.size.y - shrinkTop);
        box.offset = new Vector2(0, -shrinkTop / 2f);
    }
}
