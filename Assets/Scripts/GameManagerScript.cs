using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static bool isGameOver;
    public static string diedTo;
    public static float highScore = 0;
    public static float currentScore;
    public static bool isNewHighScore;

    public static void START_GAME()
    {
        isGameOver = false;

        currentScore = 0;

        // Load the scene with your game
        SceneManager.LoadScene("GameScene");
    }

    public static void START_TUTORIAL()
    {
        // Show the how to play screen
        SceneManager.LoadScene("TutorialMenu");
    }
    public static void QUIT_GAME()
    {
        // Quit the application
        Application.Quit();
    }

    public static void GAME_OVER(string _diedTo)
    {
        diedTo = _diedTo;

        isNewHighScore = (currentScore > highScore);
        if (isNewHighScore)
        {
            highScore = currentScore;
        }
        
        // Load the Game Over screen
        SceneManager.LoadScene("GameOver");
    }
}
