using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VictoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private AudioClip victoryAudio;
    private AudioSource audioSource;
    public void SetVictory()
    {
        // Stop game music
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(victoryAudio);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
