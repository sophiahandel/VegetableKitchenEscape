using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasGroup canvasGroup;
    float elapsedTime;
    public float fadeTime = 1.0f;

    void Start()
    {
		Time.timeScale = 1f;
        DoFadeOut();
	}

    IEnumerator DoFadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1.0f - (elapsedTime / fadeTime));
            yield return null;
        }

        yield return null;
    }
}
