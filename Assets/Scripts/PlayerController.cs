using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public int health = 3; // Three bars of health

    private Animator anim;
    private Rigidbody rbody;

    public GameObject shield;
    public LevelEvents events;

    //private int groundContactCount = 0;
    private bool grounded = true;
    private bool doubleJumped = false;
    private float immune = 0;
    private AudioSource soundSource;
    public CanvasGroup death_menu;
    public AudioSource lose;
    public AudioClip lose_sound;

    //public bool IsGrounded
    //{
    //    get
    //    {
    //        return groundContactCount > 0;
    //    }
    //}

    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    void Awake()
    {
        // Set Animator
        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        anim.applyRootMotion = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        lose.clip = lose_sound;
        // Set Spawn location
        this.transform.position = events.getCheckpointPosition();
    }

    void Update()
    {
        float inputTurn = Input.GetAxis("Horizontal");
        float inputForward = Input.GetAxis("Vertical");
        bool inputJump = Input.GetButtonDown("Jump");



        //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground
        bool isGrounded = grounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.85f, 0f, out closeToJumpableGround);

        if (inputJump && (isGrounded || !doubleJumped))
        {
            rbody.velocity = new Vector3(rbody.velocity.x, 5, rbody.velocity.z);
            if(!isGrounded)
            {
                doubleJumped = true;
            }
        }

        anim.SetFloat("velx", inputTurn);
        anim.SetFloat("vely", inputForward);
        anim.SetBool("isFalling", !isGrounded);
    }



    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint point = collision.GetContact(0);
        if (collision.transform.gameObject.tag == "ground" && point.normal.y > 0.3)
        {
            //Debug.Log(collision.GetContact(0).normal);
            //++groundContactCount;

            grounded = true;
            doubleJumped = false;
        }

        if (collision.transform.gameObject.tag == "Enemy") {
            if (immune == 0) {
                //print(point.normal);
                //if (point.normal != Vector3.up) {
                Debug.Log("got injured...");
                this.Hurt(collision.transform.gameObject);
                rbody.AddForce(point.normal * 210 + new Vector3(0, 10, 0));
                //}
            }
            immune++;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint point = collision.GetContact(0);
        if (collision.gameObject.tag == "ground" && point.normal.y > 0.3)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (collision.transform.gameObject.tag == "ground")
        {
            grounded = false;
        }

        if (collision.transform.gameObject.tag == "Enemy")
        {
            immune--;
        }
    }


    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        bool isGrounded = grounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.85f, 0f, out closeToJumpableGround);

        if (isGrounded)
        {
            //use root motion as is if on the ground
            newRootPosition = anim.rootPosition;
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        newRootPosition = anim.rootPosition;

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower

        this.transform.position = newRootPosition;
        this.transform.rotation = newRootRotation;


    }

    public void AddShield()
    {
        shield.SetActive(true);
    }

    public void RemoveShield()
    {
        shield.SetActive(false);
    }

    public bool IsShielded()
    {
        return shield.activeSelf;
    }

    public void AddHealth()
	{
        if (health < 3)
		{
			health++;
		}
	}

    public int GetHealth()
    {
        return this.health;
    }

    public void Hurt(GameObject enemy = null)
    {
        if (enemy)
        {
            if (enemy.GetComponentInParent<AI_Enemy>() != null) {
                if (enemy.GetComponentInParent<AI_Enemy>().CurrentState != AI_Enemy.ENEMY_STATE.DEATH) { // enemy shouldn't hurt you after you kill it
                    // play the sound of getting hit by enemy stick
                    soundSource.Play();
                    this.DealDamage();
                }
            } else if (enemy.GetComponentInParent<AI_GuardMouse>() != null) {
                if (enemy.GetComponentInParent<AI_GuardMouse>().CurrentState != AI_GuardMouse.GUARD_MOUSE_STATE.DEATH) { // enemy shouldn't hurt you after you kill it

                    // play the sound of getting hit by enemy stick
                    soundSource.Play();
                    this.DealDamage();
                }
            } else 
            {
                this.DealDamage();
            }
        }
        else
        {
            this.DealDamage();
        }
    }

    /**
     * Helper for Hurt() method
     **/
    private void DealDamage()
    {
        if (this.IsShielded())
        {
            this.RemoveShield();
        }
        else if (health >= 1)
        {
            health--;
            if (health <= 0)
            {
                print("you died...");

                this.Die();

            }
        }
        else
        {
            print("you died...");

            this.Die();

        }
    }

    public void Die()
    {
        events.IncreaseDeathCount();
        Destroy(this.gameObject);
        lose.Play();
        UnityEngine.Cursor.visible = true;
        death_menu.interactable = true;
        death_menu.blocksRaycasts = true;
        death_menu.alpha = 1f;
        Time.timeScale = 0f;
    }
}
