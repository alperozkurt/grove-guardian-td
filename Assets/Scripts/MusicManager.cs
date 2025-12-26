using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Music Tracks")]
    public AudioClip menuTheme;
    public AudioClip gameTheme;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "NatureScene")
        {
            PlayMusic(gameTheme);
        }
        else
        {
            PlayMusic(menuTheme);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if(audioSource.clip != clip)
        {
            audioSource.Stop(); // stop current music
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
