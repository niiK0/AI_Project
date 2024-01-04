using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BossAI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int startingHealth;
    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; } 
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    [Header("Ranges")]
    [SerializeField] private float rangedRangeRadius = 6f;
    [SerializeField] private float meleeRangeRadius = 3f;

    [SerializeField] private Transform playerTransform;

    private Rigidbody2D rb;


    public float patrolSpeed = 4f;
    public float chasingSpeed = 5f;
    public float returningSpeed = 5f;
    public float moveDirection = 1f;

    public Transform point1R, point2R, startingPointR;
    private Vector3 point1, point2, startingPoint, currentPoint;


    public SpriteRenderer renderer;
    private Color color;
    private NavMeshAgent agent;

    private Node topNode;

    public float refreshTime = 1f;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        color = renderer.color;
    }

    void Start()
    {
        _currentHealth = startingHealth;
        InvokeRepeating("ConstructBehaviourTree", 0f, refreshTime);

        point1 = point1R.position;
        point2 = point2R.position;
        startingPoint = startingPointR.position;
        currentPoint = point2;

        rb = GetComponent<Rigidbody2D>();
    }

    private void ConstructBehaviourTree()
    {
        PatrolNode patrolNode = new PatrolNode(playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(rangedRangeRadius, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this);
        RangeNode meleeRangeNode = new RangeNode(meleeRangeRadius, playerTransform, transform);
        MeleeNode meleeNode = new MeleeNode(agent, this);

        Sequence meleeSequence = new Sequence(new List<Node> { meleeRangeNode, meleeNode });
        Sequence rangedSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        topNode = new Selector (new List<Node> { patrolNode ,meleeSequence, rangedSequence });
    }

    private void Update()
    {
        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
        }
        
    }

    private void FixedUpdate()
    {
        if (currentPoint == point2)
        {
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
        }
    }

    internal void Patrol()
    {
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
    }

    internal void SetColor(Color color)
    {
        this.color = color;
    }
}