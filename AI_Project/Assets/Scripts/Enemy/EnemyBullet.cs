using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    public ParticleSystem explosionParticle;

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
        if (!collision.transform.CompareTag("Enemy") && !collision.transform.CompareTag("Coin"))
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.GetComponent<Health>().TakeDamage();
            }

            ParticleSystem particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            particle.Play();
            Destroy(gameObject);
        }
    }
}
