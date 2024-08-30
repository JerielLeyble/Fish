using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishScript : MonoBehaviour
{
    [Header("Fish Settings")]
    public static float defaultMoveSpeed = 3f;
    public float rotationSpeed = 7f;
    public float inkLockoutTime = 0.1f;
    public float inkSlowdownTime = 5f;

    private bool inkLockout = false;
    private bool inkTimerStart = false;
    private bool inked = false;
    private float inkTimer;
    private float moveSpeed = defaultMoveSpeed;
    public Animator transition;

    // Update is called once per frame
    void Update()
    {
        if (!GameManagerScript.isGameOver){
            // Get the current screen position of the mouse from Input
            Vector3 mousePos2D = Input.mousePosition;

            // The Camera's z position sets how far to push the mouse into 3D
            mousePos2D.z = -Camera.main.transform.position.z;

            // Convert the point from 2D screen space into 3D game world space
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos2D);

            // Smoothly move towards the mouse position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            // Rotate towards the mouse direction
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Timer start for ink lockout
            if (inkLockout && !inkTimerStart)
            {
                inkTimerStart = true;
                inkTimer = Time.time;
            }

            // End ink lockout timer
            if (Time.time - inkTimer > inkLockoutTime)
            {
                inkLockout = false;
                inkTimerStart = false;
            }
        }
    }

    void FixedUpdate()
    {
        // End ink slowdown
        if ((Time.time - inkTimer > inkSlowdownTime) && inked)
        {
            moveSpeed = moveSpeed + (defaultMoveSpeed - moveSpeed / 50);
            if (moveSpeed >= defaultMoveSpeed)
            {
                inked = false;
            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collWith = coll.gameObject;
        string collTag = collWith.tag;

        if (collTag == "harpoon" || collTag == "Shark" || collTag == "Squid")
        {
            Destroy(collWith);
            if (!GameManagerScript.isGameOver)
                GameManagerScript.isGameOver = true;
                StartCoroutine(ANIMATION_START(collTag));
        }

        else if (collTag == "InkBlob")
        {
            Destroy(collWith);
            if (!inkLockout)
            {
                moveSpeed = moveSpeed * 0.75f;
                inkLockout = true;
                inkTimerStart = false;
                inked = true;
            }
        }
    }
    

    IEnumerator ANIMATION_START(string collTag)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);
        
        GameManagerScript.GAME_OVER(collTag);
        
    }
}