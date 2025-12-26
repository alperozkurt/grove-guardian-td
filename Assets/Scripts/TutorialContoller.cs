using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialContoller : MonoBehaviour
{
    public void FinishStory()
    {
        SceneManager.LoadScene("NatureScene");
    }
}
