using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSM : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Shoot,
        Melee,
    }

    public Animator anim;
    
    public EnemyState state = EnemyState.Idle;
    
    private Transform player;

    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
