using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public LevelEvents events;
    public int spawnNumber;
    public AudioClip CheckpointSound;
    public AudioSource CheckpointSource;
    bool collected;
    public GameObject tube;

    private void Start()
    {
        CheckpointSource.clip = CheckpointSound;
        collected = false;
    }

    /**private void LateUpdate()
    {
        if (events.getState().currentCheckpoint >= spawnNumber)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }**/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            CheckpointSource.Play();
            collected = true;
            StartCoroutine(fadeOut(tube, 2));
            if (events.getState().currentCheckpoint < spawnNumber)
            {
                events.nextCheckpoint();
            }
        }
    }

    IEnumerator fadeOut(GameObject objectToFade, float duration)
    {
        float counter = 0f;

        float a, b;
         a = 1;
         b = 0;

        Color currentColor = Color.clear;

        MeshRenderer tempRenderer = objectToFade.GetComponent<MeshRenderer>();
        
        if (tempRenderer != null)
        {
            currentColor = tempRenderer.material.color;

            //ENABLE FADE Mode on the material if not done already
            tempRenderer.material.SetFloat("_Mode", 2);
            tempRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            tempRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            tempRenderer.material.SetInt("_ZWrite", 0);
            tempRenderer.material.DisableKeyword("_ALPHATEST_ON");
            tempRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            tempRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            tempRenderer.material.renderQueue = 3000;
        }
        else
        {
            yield break;
        }

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            tempRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        }
        yield return new WaitForSeconds(1);
        Destroy(tube);
    }
}
