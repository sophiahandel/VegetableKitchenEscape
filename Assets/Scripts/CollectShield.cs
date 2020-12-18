using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectShield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.tag.Equals("Player"))
        {
            PlayerController pc = o.GetComponent<PlayerController>();
            if (!pc.IsShielded())
            {
                pc.AddShield();
                Destroy(this.gameObject);
            }
        }
    }
}
