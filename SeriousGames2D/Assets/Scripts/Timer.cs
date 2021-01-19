using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameManager manager;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endGameTimeText;
    
    public float time;
    bool isCounting = false;

    void Start()
    {
        Invoke("StartTimer", 3);
    }

    void Update()
    {
        if(isCounting && !manager.gameEnded)
            time -= Time.deltaTime;  

        if(timerText != null)
        {
            timerText.text = time.ToString("F2").Replace(".", ":");
            endGameTimeText.text = time.ToString("F2").Replace(".", ":");
            manager.setTime(time);
        }

        if (time < 0)
        {
            time = 0;
            isCounting = false;
            manager.setTime(0);
            manager.EndGame(false);
        }
    }

    void StartTimer()
    {
        isCounting = true;
    }
}
