using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    public float lifeTime = 3f;

    private Vector2 targetDirection;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            targetDirection =
                (player.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        transform.Translate(
            targetDirection * speed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            Destroy(gameObject);
        }
    }
}