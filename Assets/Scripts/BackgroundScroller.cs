using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{

    public float speed = 0.5f; //how fast the background slides
    private float tileWidth; //width of one image
    private float startX; //where the layer started

    void Start()
    {
        tileWidth = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x; //reads the real width of the first image
        startX = transform.localPosition.x;
    }

    void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime; //which slides the layer to the right each frame
        if (transform.localPosition.x >= startX + tileWidth)
        {
            transform.localPosition -= Vector3.right * tileWidth; //when layer moves one image width, it jumps back by the same width, so jump is invisible with 2 copies
        }
    }
}
