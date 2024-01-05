using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public Rigidbody2D targetRb;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    public Transform groundCheck;
    public LayerMask groundCheckMask;
    public SpriteRenderer renderer;
    public float stopDistance;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    private Collider2D col;   
    

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col= GetComponent<Collider2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }


    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if(followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        else
        {
            seeker.CancelCurrentPathRequest();
        }
    }

    private void PathFollow()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        
        if(Vector2.Distance(transform.position, target.position) <= stopDistance)
        {
            return;
        }

        //RaycastHit2D isGrounded = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0, Vector2.down, 0.1f, groundCheckMask);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force, ForceMode2D.Force);
        Debug.Log("Moved");
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Mudança de direção do visual
        if (directionLookEnabled)
        {
            if(rb.velocity.x < -0.05f)
            {
                renderer.flipX = true;
            }
            else if(rb.velocity.x > 0.05f)
            {
                renderer.flipX = false;
            }
        }
    }

    public void KnockBack(Vector2 kbForce)
    {
        rb.AddForce(kbForce, ForceMode2D.Impulse);
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
