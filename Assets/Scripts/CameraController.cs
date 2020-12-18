using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    private Vector3 verticalOffset;

    private Animator animator;
    private float angleBehind;
    private float angleUp;
    private int layerMask = 1 << 11;


    // Start is called before the first frame update
    private void Start()
    {
        offset = new Vector3(0, 2, -2);
        verticalOffset = new Vector3(0, 1f, 0);

        angleBehind = 0;

        layerMask = ~layerMask;

        Cursor.visible = false;

        animator = player.GetComponent<Animator>();
        if (animator == null)
            Debug.Log("Animator could not be found");
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        float mouseDx = Input.GetAxis("Mouse X") / 5;
        float mouseDy = Input.GetAxis("Mouse Y") / 5;

        float joyDx = Input.GetAxis("CamHorizontal") * 5;
        float joyDy = Input.GetAxis("CamVertical") * 5;

        //player.transform.rotation.eulerAngles.y;

        angleBehind += (joyDx + mouseDx) * 10;
        angleUp += (joyDy + mouseDy) * 10;


        if (animator.GetFloat("vely") > 0 || animator.GetFloat("velx") != 0)
        {
            angleBehind = Mathf.LerpAngle(angleBehind, player.transform.rotation.eulerAngles.y, 0.05f);
            angleUp = Mathf.LerpAngle(angleUp, 0, 0.025f);
        }


        transform.position = player.transform.position + (Quaternion.AngleAxis(angleBehind, Vector3.up) * offset);
        transform.LookAt(player.transform.position + verticalOffset);
        transform.rotation *= Quaternion.AngleAxis(angleUp, Vector3.left);

        // Vector3 diff = player.transform.position - transform.position;
        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, diff, out hit, diff.magnitude, layerMask))
        // {
        //     print("hit something with camera...");
        //     // Debug.DrawRay(transform.position, diff.normalized * hit.distance, Color.yellow);
        //     // transform.position = transform.position + diff.normalized * hit.distance;
        // }
    }
}