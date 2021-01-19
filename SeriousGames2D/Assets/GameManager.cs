using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject levelComplete;
    public TextMeshProUGUI StrikeText;
    public TextMeshProUGUI OutcomeText;
    public AudioSource musicPlayer;

    float finalTime = 0;
    int strikeCount = 0;
    public bool gameEnded = false;

    public void EndGame(bool victory)
    {
        if(gameEnded == false)
        {
            gameEnded = true;
            levelComplete.SetActive(true);
            if (victory)
            {
                OutcomeText.text = "Complete";
                OutcomeText.fontSize = 25;
                OutcomeText.color = new Color(79, 154, 78);
                FindObjectOfType<Medals>().setMedal(finalTime);
            }
            else
            {
                OutcomeText.text = "Failed";
                OutcomeText.fontSize = 37.5f;
                OutcomeText.color = new Color(154, 78, 82);
            }
            musicPlayer.Stop();
        }
    }

    public void Strike()
    {
        strikeCount++;
        StrikeText.text = StrikeText.text + "X   ";

        StartCoroutine(FindObjectOfType<Camera>().GetComponent<CameraScript>().Shake(0.15f, 0.2f));
        GetComponent<AudioSource>().Play();

        // check if over strike limit
        if (strikeCount >= 3)
            EndGame(false);
    }

    public void setTime(float t)
    {
        finalTime = t;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }  
    
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
