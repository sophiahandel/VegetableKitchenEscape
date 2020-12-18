using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    private PlayerController pc;
    private float timer = 0;
    private bool hurtPlayer = false;
    public float delay = 1;

    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (hurtPlayer)
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                pc.Hurt();
                timer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Player")
        {
            timer = 0;
            pc.Hurt();
            hurtPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Player")
        {
            hurtPlayer = false;
        }
    }
}
