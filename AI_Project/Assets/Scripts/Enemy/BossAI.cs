using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BossAI : MonoBehaviour
{
    [Header("Ranges")]
    [SerializeField] private float rangedRangeRadius = 6f;

    [SerializeField] private Transform playerTransform;

    private Rigidbody2D rb;

    public float patrolSpeed = 4f;
    public float chasingSpeed = 5f;
    public float returningSpeed = 5f;
    public float moveDirection = 1f;

    public Transform point1R, point2R;
    private Vector3 point1, point2, currentPoint;

    public SpriteRenderer renderer;

    public float attackCw;
    private float attackCwInt;

    private Node topNode;

    public float refreshTime = 1f;

    void Start()
    {
        InvokeRepeating("ConstructBehaviourTree", 0f, refreshTime);

        point1 = point1R.position;
        point2 = point2R.position;
        currentPoint = point2;
        moveDirection = 1;

        attackCwInt = attackCw;

        rb = GetComponent<Rigidbody2D>();
    }

    private void ConstructBehaviourTree()
    {


        //Move node
        PatrolNode moveToNextPointNode = new PatrolNode(transform, currentPoint, this);
        //Find next point node
        PatrolPointNode findNextPointNode = new PatrolPointNode(new Vector3[]{ point1, point2}, currentPoint, this);

        //Ranged att node
        ShootNode shootNode = new ShootNode(attackCwInt, this);
        //Check range node
        RangeNode shootingRangeNode = new RangeNode(rangedRangeRadius, playerTransform, transform);

        //patrol sequence node
        Sequence patrolSequence = new Sequence(new List<Node> { findNextPointNode, moveToNextPointNode });
        //found player sequence
        Sequence IsPlayerInAttackRangeSequenceNode = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        topNode = new Selector (new List<Node> { IsPlayerInAttackRangeSequenceNode, patrolSequence });
    }

    private void Update()
    {
        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
        }

        if(attackCwInt > 0)
        {
            attackCwInt -= 1 * Time.deltaTime;
        }

        if (moveDirection == 1) renderer.flipX = true;
        else renderer.flipX = false;

    }

    public Vector3 GetCurrentPoint()
    {
        return currentPoint;
    }

    public void SetCurrentPoint(Vector3 cP)
    {
        currentPoint = cP;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(point1R.position, 0.5f);
        Gizmos.DrawWireSphere(point2R.position, 0.5f);
        Gizmos.DrawLine(point1R.position, point2R.position);
    }

    internal void Shoot()
    {
        GetComponent<BossAttack>().Shoot();
        attackCwInt = attackCw;
    }

    internal void Patrol()
    {
        if (currentPoint == point1)
        {
            moveDirection = -1;
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
        }

        if (currentPoint == point2)
        {
            moveDirection = 1;
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
        }
    }

    internal void SetColor(Color color)
    {
        renderer.color = color;
    }
}