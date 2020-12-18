using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCreditsScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject creditsPanel;

    public void OpenCredits()
    {
        if(creditsPanel != null && mainPanel != null)
        {
            creditsPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
    }

    public void CloseCredits()
    {
        if(creditsPanel != null && mainPanel != null)
        {
            mainPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }
    }
}
