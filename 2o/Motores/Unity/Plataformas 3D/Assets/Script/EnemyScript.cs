using System;
using NUnit.Framework.Internal;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float health = 25;
    public float damage = 10;
    public float attackDistance = 3;
    public float speed = 1;
    public Vector3 playerPos;
    public float playerDistance;
    public bool canTrack = false;
    public bool canAttack = true;
    public float attackCooldown = 0f;
    public float maxAttackCooldown = 3f;
    
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Timers();
        Actions();
    }

    void Timers()
    {
        if (!canAttack)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                canAttack = true;
            }
        }
    }

    void Actions()
    {
        if (canTrack)
        {
            playerDistance = Vector3.Distance(transform.position, playerPos);
            if (Vector3.Distance(transform.position, playerPos) > attackDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
            }
            else if (canAttack)
            {
                Attack();
            }
            transform.LookAt(playerPos);
        }
    }

    void Attack()
    {
        canAttack = false;
        attackCooldown = maxAttackCooldown;
        animator.SetTrigger("Attack");
    }

    public void TrackPlayer(Vector3 pos)
    {
        playerPos = pos;
        canTrack = true;
    }

    public void DontTrackPlayer()
    {
        playerPos = Vector3.zero;
        canTrack = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}