using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [Header("--- Thông số cơ bản ---")]
    public float speed = 2.5f;
    public float health = 3f;

    [Header("--- Tiến hóa ---")]
    public float healthBonusPerMinute = 2f;
    public float speedBonusPerMinute = 0.2f;

    [Header("--- Đẩy lùi (Knockback) ---")]
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    public float stopDistance = 0.8f;

    [Header("--- Tấn công ---")]
    public int damage = 10;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    [Header("--- Vật phẩm & Hiệu ứng ---")]
    public GameObject expGemPrefab;

    // --- Biến nội bộ ---
    private float currentHealth;
    private float currentSpeed;
    private float knockbackCounter;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Tính toán tiến hóa theo thời gian
        float minutesPlayed = Time.timeSinceLevelLoad / 60f;
        currentHealth = health + (healthBonusPerMinute * minutesPlayed);
        currentSpeed = speed + (speedBonusPerMinute * minutesPlayed);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        // 1. Lật ảnh theo vị trí của Player (Đã đảo chiều để không bị ngược hướng)
        if (player.position.x < transform.position.x)
            sr.flipX = false;  // Player ở bên trái
        else
            sr.flipX = true;   // Player ở bên phải

        // 2. Logic đẩy lùi (Knockback)
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            if (anim != null) anim.SetBool("Run", false);
            return;
        }

        // 3. Di chuyển và Tấn công
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Đuổi theo Player và chạy animation Run
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * currentSpeed;

            if (anim != null) anim.SetBool("Run", true);
        }
        else
        {
            // Đã áp sát -> Dừng lại cắn
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("Run", false);

            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void AttackPlayer()
    {
        if (anim != null) anim.SetTrigger("Attack");

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
    }

    // --- XỬ LÝ VA CHẠM (Nhận sát thương từ Đạn và Kiếm Xoay) ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // 1. Trúng Đạn
        BulletScript dan = collision.GetComponent<BulletScript>();
        if (dan == null) dan = collision.GetComponentInParent<BulletScript>();

        if (dan != null)
        {
            ApplyKnockback(dan.transform.position);
            TakeDamage(1);
            Destroy(collision.gameObject);
            return;
        }

        // 2. Trúng Kiếm Xoay (SwordOrbit)
        SwordOrbit kiemXoay = collision.GetComponent<SwordOrbit>();
        if (kiemXoay == null) kiemXoay = collision.GetComponentInParent<SwordOrbit>();

        if (kiemXoay != null)
        {
            ApplyKnockback(kiemXoay.transform.position);
            TakeDamage(2); // Trừ 2 máu khi dính kiếm xoay
        }
    }

    void ApplyKnockback(Vector3 damageSourcePosition)
    {
        knockbackCounter = knockbackTime;
        Vector2 difference = (transform.position - damageSourcePosition).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(difference * knockbackForce, ForceMode2D.Impulse);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Rớt ngọc kinh nghiệm
        if (expGemPrefab != null)
        {
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 0.2f);
    }
}