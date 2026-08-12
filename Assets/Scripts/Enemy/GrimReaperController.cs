using UnityEngine;

public class GrimReaper : MonoBehaviour
{
    [Header("--- Thông số cơ bản ---")]
    public float speed = 2.0f;
    public float health = 10f;

    [Header("--- Tiến hóa ---")]
    public float healthBonusPerMinute = 5f;
    public float speedBonusPerMinute = 0.3f;

    [Header("--- Đẩy lùi (Knockback) ---")]
    public float knockbackForce = 6f;
    public float knockbackTime = 0.2f;
    public float stopDistance = 1.2f;

    [Header("--- Tấn công ---")]
    public int damage = 25;
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;

    [Header("--- Vật phẩm & Hiệu ứng ---")]
    public GameObject expGemPrefab;

    // --- Biến nội bộ ---
    private float currentHealth;
    private float currentSpeed;
    private float knockbackCounter;
    private bool isDead = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Tăng sức mạnh theo thời gian sống sót của người chơi
        float minutesPlayed = Time.timeSinceLevelLoad / 60f;
        currentHealth = health + (healthBonusPerMinute * minutesPlayed);
        currentSpeed = speed + (speedBonusPerMinute * minutesPlayed);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        // 1. Lật mặt theo hướng Player
        if (player.position.x < transform.position.x)
            sr.flipX = false; // Sửa thành true nếu sprite gốc bị ngược
        else
            sr.flipX = true;  // Sửa thành false nếu sprite gốc bị ngược

        // 2. Chịu lực đẩy lùi (Knockback)
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            if (anim != null) anim.SetBool("IsWalking", false);
            return;
        }

        // 3. Di chuyển đuổi theo & Tấn công
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Nếu ở xa -> Chạy tới
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * currentSpeed;

            if (anim != null) anim.SetBool("IsWalking", true);
        }
        else
        {
            // Áp sát -> Dừng lại và chém
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("IsWalking", false);

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

    // --- XỬ LÝ VA CHẠM NHẬN SÁT THƯƠNG ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // 1. Dính đạn
        BulletScript dan = collision.GetComponent<BulletScript>();
        if (dan == null) dan = collision.GetComponentInParent<BulletScript>();

        if (dan != null)
        {
            ApplyKnockback(dan.transform.position);
            TakeDamage(1);
            Destroy(collision.gameObject);
            return;
        }

        // 2. Dính kiếm xoay
        SwordOrbit kiemXoay = collision.GetComponent<SwordOrbit>();
        if (kiemXoay == null) kiemXoay = collision.GetComponentInParent<SwordOrbit>();

        if (kiemXoay != null)
        {
            ApplyKnockback(kiemXoay.transform.position);
            TakeDamage(2); // Trừ 2 máu
        }
    }

    void ApplyKnockback(Vector3 sourcePosition)
    {
        knockbackCounter = knockbackTime;
        Vector2 difference = (transform.position - sourcePosition).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(difference * knockbackForce, ForceMode2D.Impulse);
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero; // Dừng hẳn lại
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; // Tắt va chạm

        // Rớt ngọc kinh nghiệm
        if (expGemPrefab != null)
        {
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        // Bốc hơi sau 0.2s
        Destroy(gameObject, 0.2f);
    }
}