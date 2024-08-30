using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAnimation : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    private SpriteRenderer spriteRenderer;
    private Quaternion originalRotation; // Store the original rotation

    // Start is called before the first frame update
     void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the GameObject
        originalRotation = transform.rotation; // Store the original rotation
        StartCoroutine(SwitchSpriteCoroutine()); // Start the coroutine to switch sprites
    }

    void Update()
    {
        originalRotation = transform.rotation; // Store the original rotation
    }

    private IEnumerator SwitchSpriteCoroutine()
    {
        while (true)
        {
            // Toggle between sprite1 and sprite2
            if (spriteRenderer.sprite == sprite1)
            {
                spriteRenderer.sprite = sprite2;
            }
            else
            {
                spriteRenderer.sprite = sprite1;
            }

            // Reapply the original rotation
            transform.rotation = originalRotation;

            // Wait for 0.5 seconds
            yield return new WaitForSeconds(0.3f);
        }
    }
}