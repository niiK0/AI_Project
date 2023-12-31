using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    // Set the direction of the bullet
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bullet in the set direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Destroy the bullet when it goes out of screen
        if (!GetComponentInChildren<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }
}
