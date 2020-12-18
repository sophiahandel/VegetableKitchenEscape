using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
		Time.timeScale = 1f;
		SceneManager.LoadScene("Kitchen_Level");
	}
}
