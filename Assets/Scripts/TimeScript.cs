using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeScript : MonoBehaviour
{
    public Text timeText;

    void Update()
    {
        // Update elapsed time
        if (!GameManagerScript.isGameOver)
        {
            GameManagerScript.currentScore += Time.deltaTime;
        }

         // Update the Text component with the formatted time
        timeText.text = "Time: " + FORMAT_TIME(GameManagerScript.currentScore);
    }

    public static string FORMAT_TIME(float time)
    {
        
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt(time * 100f % 100f);

        return string.Format($"{minutes:00}:{seconds:00}.{milliseconds:00}");
    }
}
