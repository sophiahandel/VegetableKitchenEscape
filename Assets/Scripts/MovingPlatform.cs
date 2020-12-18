using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private GameObject player;
    private Animator anim;
    private Vector3 initPos;
    private bool playerOn;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        playerOn = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            initPos = player.transform.localPosition;
        }
        else if(playerOn)
        {
            //Vector3 localY = new Vector3(0, player.transform.localPosition.y, 0);
            player.transform.localPosition = initPos;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = transform;
            initPos = player.transform.localPosition;
            playerOn = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = null;
            playerOn = false;
        }
    }
}
