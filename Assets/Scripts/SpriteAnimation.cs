using UnityEngine;
using UnityEngine.UI; // Required for UI elements!

public class SpriteAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public Sprite[] animationFrames;
    public float frameRate = 12f;
    
    // Change this from SpriteRenderer to Image
    private Image uiImage; 
    private float timer;
    private int currentFrameIndex;

    void Awake()
    {
        // Get the Image component instead
        uiImage = GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrameIndex++;

            if (currentFrameIndex >= animationFrames.Length)
            {
                currentFrameIndex = 0;
            }

            // Assign the sprite to the UI Image
            if (uiImage != null)
            {
                uiImage.sprite = animationFrames[currentFrameIndex];
            }
        }
    }
}