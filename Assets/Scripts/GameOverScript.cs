using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public GameObject harpoonObj;
    public GameObject sharkObj;
    public GameObject squidObj;

    public Color highScoreColor;
    public Text gameOverText;
    public Text highScoreText;
    public Text yourScoreText;

    // Start is called before the first frame update
    void Start()
    {
        ShowDiedToSprite();
        UpdateScores();
    }

    void ShowDiedToSprite()
    {
        // Show the sprite of the thing that killed the player
        if (GameManagerScript.diedTo == "harpoon")
        {
            foreach (var r in harpoonObj.GetComponentsInChildren<Renderer>(true))
            {
                r.enabled = true;
            }
        }
        else if (GameManagerScript.diedTo == "Squid")
        {
            squidObj.GetComponent<Renderer>().enabled = true;
        }
        else if (GameManagerScript.diedTo == "Shark")
        {
            sharkObj.GetComponent<Renderer>().enabled = true;
        }
    }

    void UpdateScores()
    {
        highScoreText.text = TimeScript.FORMAT_TIME(GameManagerScript.highScore);
        yourScoreText.text = TimeScript.FORMAT_TIME(GameManagerScript.currentScore);

        if (GameManagerScript.isNewHighScore)
        {
            highScoreText.color = highScoreColor;
            yourScoreText.color = highScoreColor;
            gameOverText.text = "NEW HIGH SCORE!";
        }
    }
}
