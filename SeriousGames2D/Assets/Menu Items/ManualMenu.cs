using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMenu : MonoBehaviour
{
    public GameObject[] Pages;
    public GameObject prevButton;
    public GameObject nextButton;

    int currentpage = 0;

    public void NextPage()
    {
        if (currentpage == 0) //Enable Previous Button if leaving first page
        {
            prevButton.SetActive(true);
        }

        Pages[currentpage].SetActive(false);

        currentpage++;

        Pages[currentpage].SetActive(true);

        if (currentpage == Pages.Length-1) //Disable Next Button if on last page
        {
            nextButton.SetActive(false);
        }
    }

    public void PrevPage()
    {
        if (currentpage == Pages.Length-1) //Enable Next Button if leaving last page
        {
            nextButton.SetActive(true);
        }

        Pages[currentpage].SetActive(false);

        currentpage--;

        Pages[currentpage].SetActive(true);

        if (currentpage == 0) //Disable Previous Button if on first page
        {
            prevButton.SetActive(false);
        }
    }    

    public void GoBack()
    {

    }
}
