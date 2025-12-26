using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryContoller : MonoBehaviour
{
    [SerializeField] private GameObject[] storyImages;
    private int currentIndex = 0;

    void Start()
    {
        currentIndex = 0;
        UpdateImageVisibility();
    }

    public void OnNextButtonPressed()
    {
        currentIndex++;

        if(currentIndex >= storyImages.Length)
        {
            FinishStory();
        }
        else
        {
            UpdateImageVisibility();
        }
    }

    void UpdateImageVisibility()
    {
        for(int i = 0; i < storyImages.Length; i++)
        {
            // if i matches currentIndex set active to true
            storyImages[i].SetActive(i == currentIndex);
        }
    }

    void FinishStory()
    {
        SceneManager.LoadScene("Tutorial");
    }

}
