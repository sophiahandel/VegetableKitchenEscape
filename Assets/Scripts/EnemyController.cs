using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Text gameText;
    public float speed = 1.0f;

    private GameObject target;
    private Rigidbody rb;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        gameText.text = "";
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(targetPos);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == target)
        {
            ContactPoint point = other.GetContact(0);
            if (point.normal == Vector3.down)
            {
                Destroy(gameObject);
            }
            else
            {
                gameText.text = "You Lose!!!";
            }
        } 
    }
}
