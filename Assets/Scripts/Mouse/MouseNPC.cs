using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Collision Detection for death handled in /detectHeadCollision.cs

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(GameObject))]
public class MouseNPC : MonoBehaviour
{
    Animator animator;
    UnityEngine.AI.NavMeshAgent navMeshAgent;

    // Use this for initialization
    void Awake() {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void setSpeedForwardAnimations(float magn, float speed) {
        animator.SetFloat("Speed", magn / speed);
    }

    public void attack(bool b) {
        if (animator == null) {
            print("animator is null");
        }
        animator.SetBool("Attack", b);
    }
    public void die() {
        StartCoroutine(deathAnimation());
    }

    // --------------------------------------
    private IEnumerator deathAnimation() {
        animator.Play("GetHitDeathBlow");
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
