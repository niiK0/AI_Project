using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackCw;
    public float shootMinDistance;
    public GameObject enemyBullet;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 0f, attackCw);
    }

    private void Shoot()
    {
        if (Vector2.Distance(target.position, transform.position) >= shootMinDistance) return;
        Vector3 direction = target.position - transform.position;
        Vector3 rotation = transform.position - target.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        // Instantiate a bullet at the player's position
        GameObject bullet = Instantiate(enemyBullet, transform.position, Quaternion.Euler(0, 0, -rotZ + 90));

        // Access the Bullet script and set its direction
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }
}
