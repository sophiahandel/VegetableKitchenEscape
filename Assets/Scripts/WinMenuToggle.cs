using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]

public class WinMenuToggle : MonoBehaviour
{
	private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    public AudioSource winSource;
    public AudioClip win;
    private bool done;

	void Awake()
	{
        done = false;
        winSource.clip = win;
		Time.timeScale = 1f;
		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			Debug.LogError("failed to find canvas group");
		}
	}

	//Update is called once per frame
	void Update()
	{
		if (GameObject.FindGameObjectWithTag("Finish").GetComponent<Win>().isTouched)
		{
            if (!done)
            {
                winSource.Play();
            }
			UnityEngine.Cursor.visible = true;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.alpha = 1f;
			Time.timeScale = 0f;
            done = true;
		}
	}
}