using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("NatureScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
