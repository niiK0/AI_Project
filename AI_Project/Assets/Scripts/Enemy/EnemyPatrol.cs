using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Patroling,
    Chasing,
}

public class EnemyPatrol : MonoBehaviour
{
    public float patrolSpeed = 4f;
    public float chasingSpeed = 5f;
    public float returningSpeed = 5f;
    public float jumpForce = 4f;

    public Vector2 detectionSize = new Vector2(7f, 1f);
    public LayerMask detectionLayerMask;
    public float moveDirection = 1f;

    public float timeToChangeDir;
    private float changeDirectionTimer;

    public Animator anim;

    public EnemyState state = EnemyState.Patroling;

    private Transform player;

    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    public Transform point1R, point2R, startingPointR;
    private Vector3 point1, point2, startingPoint, currentPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        point1 = point1R.position;
        point2 = point2R.position;
        startingPoint = startingPointR.position;
        currentPoint = point2;
        moveDirection = 1;
        anim.SetBool("Moving", true);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, detectionSize);
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(point1R.position, 0.5f);
        Gizmos.DrawWireSphere(point2R.position, 0.5f);
        Gizmos.DrawWireSphere(startingPointR.position, 0.5f);
        Gizmos.DrawLine(point1R.position, startingPointR.position);
        Gizmos.DrawLine(point2R.position, startingPointR.position);
    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.Chasing:
                if (Vector2.Distance(transform.position, player.position) <= 0.1f)
                {
                    Debug.Log("Hit the player");
                }
                break;

            case EnemyState.Patroling:
                if (Vector2.Distance(transform.position, currentPoint) < 0.5f && currentPoint == point1)
                {
                    currentPoint = point2;
                    moveDirection = 1;
                }
                if (Vector2.Distance(transform.position, currentPoint) < 0.5f && currentPoint == point2)
                {
                    currentPoint = point1;
                    moveDirection = -1;
                }

                break;
        }

        if (moveDirection == -1) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        if (Physics2D.OverlapBox(transform.position, detectionSize, 0f, detectionLayerMask) != null)
        {
            if (state == EnemyState.Patroling || state == EnemyState.Idle)
            {
                EnterChasingState();
            }

        }
        else if (state == EnemyState.Chasing)
        {
            EnterPatrolState();
        }
    }

    public void KnockBack(Vector2 kbForce)
    {
        rb.AddForce(kbForce, ForceMode2D.Impulse);
    }

    private void EnterIdleState()
    {
        state = EnemyState.Idle;
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    private void EnterPatrolState()
    {
        state = EnemyState.Patroling;
        moveDirection = currentPoint == point1 ? -1 : 1;
    }

    private void EnterChasingState()
    {
        state = EnemyState.Chasing;
        moveDirection = player.position.x < transform.position.x ? -1 : 1;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case EnemyState.Chasing:
                rb.velocity = new Vector2(chasingSpeed * moveDirection, rb.velocity.y);
                break;

            case EnemyState.Patroling:
                if (currentPoint == point2)
                {
                    rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            InvokeRepeating("DoDamage", 0f, 1f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (IsInvoking()) CancelInvoke();
        }
    }

    public void DoDamage()
    {
        player.GetComponent<Health>().TakeDamage();
    }
}