using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Medals : MonoBehaviour
{
    public GameObject Bronze;
    public float bronzeLimit;
    public GameObject Silver;
    public float silverLimit;
    public GameObject Gold;
    public float goldLimit;
    public GameObject MedalText;

    public void setMedal(float time)
    {
        TextMeshProUGUI mText = MedalText.GetComponent<TextMeshProUGUI>();
        MedalText.SetActive(true);
        if(time > goldLimit)
        {
            // Set Gold Medal
            Gold.SetActive(true);
            mText.text = "Gold Medal!";
        }
        else if(time > silverLimit)
        {
            // Set Silver Medal
            Silver.SetActive(true);
            mText.text = "Silver Medal!\nBeat it before " + goldLimit + " seconds for Gold!";
        }
        else if (time > bronzeLimit)
        {
            // Set Bronze Medal
            Bronze.SetActive(true);
            mText.text = "Bronze Medal!\nBeat it before " + silverLimit + " seconds for Silver!";
        }
        else
        {
            // Set No Medal
            mText.text = "No Medal!\nBeat it before " + bronzeLimit + " seconds for Bronze!";
        }
    }
}
