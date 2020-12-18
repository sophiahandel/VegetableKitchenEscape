using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartAction : MonoBehaviour
{
    public LevelEvents level;
    public CanvasGroup canvasGroup;
    public void Restart()
    {
        level.GetComponent<LevelEvents>().ReloadScene();
        Time.timeScale = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }
}
