using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AudioClip gameOverAudio;
    private AudioSource audioSource;

    public void SetGameOver()
    {
        // Stop game music
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(gameOverAudio);
    }

    public void Quit()
    {
        Application.Quit();
    }
}