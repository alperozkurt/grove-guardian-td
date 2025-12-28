using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AudioClip gameOverAudio;
    private AudioSource audioSource;

    public void SetGameOver()
    {
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