using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public bool isTouched;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isTouched = false;
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            isTouched = true;
        }
    }
}
