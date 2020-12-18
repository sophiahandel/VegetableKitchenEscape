using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKnives : MonoBehaviour
{
    public Vector3 center;
    public Vector3 size;
    public float spawnDelay = 2f;

    public GameObject KnifePrefab;

    private float timer;
    private bool enter;

    private void Start()
    {
        enter = false;
        timer = 0;
    }

    private void Update()
    {
        if (enter)
        {
            timer += Time.deltaTime;
            if (timer > spawnDelay)
            {
                this.Spawner();
                timer = 0;
            }
        }
    }

    public void Spawner()
    {
        if (enter)
        {
            Vector3 pos = transform.localPosition + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));

            Instantiate(KnifePrefab, pos, Quaternion.Euler(90, 90, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            enter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            enter = false;
            timer = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.localPosition + center, size);
    }
}
