using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectHeadCollision : MonoBehaviour
{
    public GameObject mouse;
    MouseNPC mouseNPC;
    AI_Enemy ai_enemy;
    private AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        mouseNPC = mouse.GetComponent<MouseNPC>();
        ai_enemy = mouse.GetComponent<AI_Enemy>();
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collision Detection Test
    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.CompareTag("Player") && ai_enemy.CurrentState != AI_Enemy.ENEMY_STATE.DEATH)
        {
            mouseNPC.die();
            ai_enemy.deathHasOccured();

            // play the sound
            soundSource.Play();
        }
    }
}
