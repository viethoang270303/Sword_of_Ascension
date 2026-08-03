using UnityEngine;
using System.Collections;

public class EnemyRunOnly : MonoBehaviour
{
    [Header("--- Chỉ số cơ bản ---")]
    public int maxHealth = 50;
    private int currentHealth;
    public float moveSpeed = 3f;

    [Header("--- Tấn công ---")]
    public int damage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    [Header("--- Tầm nhìn (Đuổi theo) ---")]
    public float chaseRange = 5f;

    private Transform player;
    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        // Cứ mỗi khi quái ĐƯỢC SINH RA, nó sẽ tự quét bản đồ tìm người chơi
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || currentHealth <= 0) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (direction.x > 0) sr.flipX = true;
        else if (direction.x < 0) sr.flipX = false;

        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (direction.x > 0) sr.flipX = true;
        else if (direction.x < 0) sr.flipX = false;

        if (Time.time >= nextAttackTime)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;

        // Vẫn giữ lại nháy đỏ 0.15s để bạn biết là chém trúng nhé
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}