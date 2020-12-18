using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpPadEvent : MonoBehaviour
{
    public float speed = 500.0f;

    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnCollisionEnter(Collision other)
     {
        ContactPoint point = other.GetContact(0);
        if (other.gameObject == player && point.normal == Vector3.down)
         {
             other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up*speed);
             other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward*speed);
         }
     }
}
