using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHeart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.tag.Equals("Player"))
        {
            PlayerController pc = o.GetComponent<PlayerController>();
            if (pc.GetHealth() < 3)
            {
                pc.AddHealth();
                Destroy(this.gameObject);
            }
        }
    }
}
