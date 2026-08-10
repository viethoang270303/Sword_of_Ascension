using UnityEngine;

public class AutoSlash60 : MonoBehaviour
{
    [Header("Tự động chém")]
    public float attackInterval = 1.5f;
    public float attackRadius = 2f;
    [Range(0, 360)]
    public float attackAngle = 60f;
    public int damage = 1;

    [Header("Layer quái")]
    public LayerMask enemyLayer;

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = attackInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Attack();
            timer = attackInterval;
        }
    }

    void Attack()
    {
        // Hướng trước mặt nhân vật
        Vector2 forward = sr.flipX ? Vector2.right : Vector2.left;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            Vector2 dirToTarget =
                ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;

            float angle = Vector2.Angle(forward, dirToTarget);

            if (angle <= attackAngle * 0.5f)
            {
                EnemyScript enemy = hit.GetComponent<EnemyScript>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);

                    // Đẩy lùi nhẹ
                    Rigidbody2D erb = hit.GetComponent<Rigidbody2D>();
                    if (erb != null)
                    {
                        Vector2 knockDir =
                            ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;

                        erb.linearVelocity = Vector2.zero;
                        erb.AddForce(knockDir * 3f, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Vector2 forward = Vector2.right;

        if (sprite != null)
            forward = sprite.flipX ? Vector2.right : Vector2.left;

        float half = attackAngle * 0.5f;

        Vector3 leftDir = Quaternion.Euler(0, 0, half) * forward;
        Vector3 rightDir = Quaternion.Euler(0, 0, -half) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftDir * attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * attackRadius);
    }
}