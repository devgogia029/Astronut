using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{

    public float scrollSpeed = 100f; //how many pixels per second the text moves up
    public string nextScene = "MainMenu"; //the scene to load when credits end
    private RectTransform rect; //ref to this objects rect transform

    void Start()
    {
        rect = GetComponent<RectTransform>(); //finds the Rect Transform on this object and stores it in rect
    }

    // Update is called once per frame
    void Update()
    {
        rect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime; //moves the text up a little every frame; deltaTime keeps the speed the same on any computer
        if (rect.anchoredPosition.y > rect.rect.height)
            SceneManager.LoadScene(nextScene); //when the texts top edge is higher than its own height, the last line has left the screen, so go to the next scene
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(nextScene); //Esc skips the credits
    }
}
