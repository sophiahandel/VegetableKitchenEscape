using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    private LevelEvents events;

    private void Awake()
    {
        events = GameObject.Find("Level Events").GetComponent<LevelEvents>();
    }

    public void Quit()
	{
        events.ResetLevelState();
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}
}