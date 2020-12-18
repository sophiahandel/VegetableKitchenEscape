using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectHeadCollisionGuard : MonoBehaviour
{
    public GameObject mouse;
    MouseNPC mouseNPC;
    AI_GuardMouse ai_guard;
    private AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        mouseNPC = mouse.GetComponent<MouseNPC>();
        ai_guard = mouse.GetComponent<AI_GuardMouse>();
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collision Detection Test
    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.CompareTag("Player") && ai_guard.CurrentState != AI_GuardMouse.GUARD_MOUSE_STATE.DEATH)
        {
            mouseNPC.die();
            ai_guard.deathHasOccured();

            // play the sound
            soundSource.Play();
        }
    }
}
