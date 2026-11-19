using UnityEngine;

[ExecuteAlways]
public class TiledBlock : MonoBehaviour
{

    private SpriteRenderer sr;
    private BoxCollider2D box;


    void Update()
    {
        if (Application.isPlaying)
            return; //stops it running during the game
        if (sr == null)
            sr = GetComponent<SpriteRenderer>(); //looks up the renderer the first time
        if (box == null)
            box = GetComponent<BoxCollider2D>(); //same for the collider, looks up first time
        box.size = sr.size; //copies the renderer's size into the collider every time something changes
        box.offset = Vector2.zero; //keeps collider centered
    }
}
