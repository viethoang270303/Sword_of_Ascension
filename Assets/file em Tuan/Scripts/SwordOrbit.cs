using UnityEngine;

public class SwordOrbit : MonoBehaviour
{
    public float rotateSpeed = 300f;
    public float orbitRadius = 1.5f;
    public int damage = 5;

    private Transform playerTransform;
    private Transform swordSpriteTransform;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerTransform = p.transform;

        if (transform.childCount > 0)
        {
            swordSpriteTransform = transform.GetChild(0);
            swordSpriteTransform.localPosition = new Vector3(orbitRadius, 0f, 0f);
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.GetComponent<EnemyScript>() != null)
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Gọi chuẩn hàm TakeDamage mới
            }
        }
    }
}