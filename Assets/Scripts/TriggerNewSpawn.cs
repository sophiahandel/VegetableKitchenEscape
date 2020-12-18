using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNewSpawn : MonoBehaviour
{
    public LevelEvents events;
    public int spawnNumber;

    private void LateUpdate()
    {
        if (events.getState().currentCheckpoint >= spawnNumber)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (events.getState().currentCheckpoint < spawnNumber)
            {
                events.nextCheckpoint();
            }
        }
    }
}
