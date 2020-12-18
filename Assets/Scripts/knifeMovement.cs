using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class knifeMovement : MonoBehaviour
{
    public GameObject player;
    public Text gameText;
    public float speed = 1.0f;
    public float rotationSpeed = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-rotationSpeed * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            gameText.text = "You Lose!!!";
        }
    }
}
