using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public AudioClip ButtonClickSound;
    public AudioSource ButtonClickSource;
    public LevelEvents events;
    public Animator anim;

    private void Start()
    {
        anim.enabled = false;
        ButtonClickSource.clip = ButtonClickSound;

        if (events.getState().WallEventActive && this.transform.childCount > 0)
        {
            Transform child = this.transform.GetChild(0);
            child.localPosition = new Vector3(0, 0.69f, 0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.GetContact(0).normal.Equals(Vector3.down))
        {
            if (!events.getState().WallEventActive)
            {
                ButtonClickSource.Play();
                anim.enabled = true;
                events.ChangeWallState(true);
            }
        }
    }

}
