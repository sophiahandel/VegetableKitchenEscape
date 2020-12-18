using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]

public class DeathMenuToggle : MonoBehaviour
{
	private CanvasGroup canvasGroup;
	// Start is called before the first frame update
	void Awake()
	{
		Time.timeScale = 1f;
		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			Debug.LogError("failed to find canvas group");
		}
	}

	// Update is called once per frame
	public void toggle()
	{
		if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health == 0)
		{
			UnityEngine.Cursor.visible = true;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.alpha = 1f;
			Time.timeScale = 0f;
		}
	}
}
