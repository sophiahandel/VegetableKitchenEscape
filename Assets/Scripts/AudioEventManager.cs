using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{
    public EventSound3D eventSound3DPrefab;

    public AudioClip stepAudio;

    private UnityAction<Vector3> playerStepEventListener;

    void Awake()
    {
        playerStepEventListener = new UnityAction<Vector3>(playerStepEventHandler);
    }
    
    void OnEnable()
    {

        EventManager.StartListening<playerStepEvent, Vector3>(playerStepEventListener);

    }

    void OnDisable()
    {

        EventManager.StopListening<playerStepEvent, Vector3>(playerStepEventListener);
    }


    void playerStepEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);
        //print("playihng crunch");
        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.stepAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }
    }
}
