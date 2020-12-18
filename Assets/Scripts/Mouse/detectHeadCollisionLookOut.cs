using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectHeadCollisionLookOut : MonoBehaviour
{
    public GameObject mouse;
    MouseNPC mouseNPC;
    AI_LookOut ai;
    private AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        mouseNPC = mouse.GetComponent<MouseNPC>();
        ai = mouse.GetComponent<AI_LookOut>();
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collision Detection Test
    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.CompareTag("Player") && ai.CurrentState != AI_LookOut.LOOK_OUT_STATE.DEATH)
        {
            mouseNPC.die();
            ai.deathHasOccured();

            // play the sound
            soundSource.Play();
        }
    }
}
