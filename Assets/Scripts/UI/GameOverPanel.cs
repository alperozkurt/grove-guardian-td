using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
[SerializeField] private GameObject gameOverMenu;

    public void SetGameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
