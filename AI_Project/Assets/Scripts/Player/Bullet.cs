using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    public ParticleSystem explosionParticle;
    public Vector2 knockBackForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Set the direction of the bullet
    public void SetDirection(Vector3 direction)
    {
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("Coin"))
        {
            if (collision.transform.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyHealth>().TakeDamage();
                EnemyAI AIEnemy = collision.GetComponent<EnemyAI>();
                EnemyPatrol PatrolEnemy = collision.GetComponent<EnemyPatrol>();

                if (AIEnemy)
                {
                    Vector2 direction = collision.transform.position - transform.position;
                    AIEnemy.KnockBack(direction * knockBackForce);
                }
                if (PatrolEnemy)
                {
                    Vector2 direction = collision.transform.position - transform.position;
                    PatrolEnemy.KnockBack(direction * knockBackForce);
                }
            }

            if(collision.transform.CompareTag("Enemy") || collision.gameObject.layer == 3)
            {
                ParticleSystem particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(gameObject);
            }

        }
    }
}
