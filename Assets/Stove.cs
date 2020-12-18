using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public AudioClip StoveSound;
    public AudioSource StoveSource;
    public GameObject player;

    private void Start()
    {
        StoveSource.clip = StoveSound;
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().health == 0)
        {
            StoveSource.Stop();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            StoveSource.Play();
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            StoveSource.Stop();
        }
    }

}
