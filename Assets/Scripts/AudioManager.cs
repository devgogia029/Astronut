using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; //singleton instance of the AudioManager
    public AudioSource musicSource; //reference to the audio source for music
    public AudioSource sfxSource; //reference to the audio source for sound effects
    public AudioClip menuMusic; //reference to the audio clip for menu music
    public AudioClip levelMusic; //reference to the audio clip for level music
    public AudioClip flipClip, deathClip, pickupClip, portalOpenClip, levelCompleteClip, clickClip, footstepClip, landClip; //references to the audio clips for various sound effects
    public float footstepVolume = 0.3f; //volume multiplier i can tune in inspector

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); //if an instance already exists, destroy this one
            return; //exit the method
        }
        Instance = this; //set the instance to this object
        DontDestroyOnLoad(gameObject); //don't destroy this object when loading a new scene
        SceneManager.sceneLoaded += OnSceneLoaded; //subscribe to the sceneLoaded event
    }

    void Start()
    {
        if (Instance != this) return; //if its the duplicate, it will not do anything
        ChooseMusic(SceneManager.GetActiveScene().name); //choose the music based on the current scene
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChooseMusic(scene.name); //choose the music based on the loaded scene
    }

    void ChooseMusic(string sceneName)
    {
        if (sceneName == "MainMenu" || sceneName == "Credits" || sceneName == "LevelComplete")
        {
            PlayMusic(menuMusic);
        }
        else
        {
            PlayMusic(levelMusic);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return; //if the clip is already playing, do nothing
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayFlip()
    {
        PlaySFX(flipClip);
    }

    public void PlayDeath()
    {
        PlaySFX(deathClip);
    }

    public void PlayPickup()
    {
        PlaySFX(pickupClip);
    }

    public void PlayPortalOpen()
    {
        PlaySFX(portalOpenClip);
    }

    public void PlayLevelComplete()
    {
        PlaySFX(levelCompleteClip);
    }

    public void PlayClick()
    {
        PlaySFX(clickClip);
    }

    public void PlayFootstep()
    {
        sfxSource.PlayOneShot(footstepClip, footstepVolume);
    }

    public void PlayLand()
    {
        PlaySFX(landClip);
    }

    public void RestartMusic()
    {
        musicSource.Stop(); //stops the track
        musicSource.Play(); //restarts the track from the beginning
    }
}
