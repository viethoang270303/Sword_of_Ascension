using UnityEngine;

public class ScytheEnemy : MonoBehaviour
{
    [Header("--- Thông số cơ bản ---")]
    public float speed = 2.5f;
    public float health = 15f;
    public float stopDistance = 1.2f;

    [Header("--- Tiến hóa theo thời gian ---")]
    public float healthBonusPerMinute = 3f;
    public float speedBonusPerMinute = 0.2f;

    [Header("--- Đẩy lùi QUÁI (Khi quái bị đánh) ---")]
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    private float knockbackCounter;

    [Header("--- Tấn công & Đẩy lùi PLAYER ---")]
    public int damage = 20;
    public float attackCooldown = 1.5f;
    public float playerKnockbackForce = 10f; // Lực hất văng Player khi xài đòn Attack
    private float nextAttackTime = 0f;

    [Header("--- Vật phẩm rớt ra ---")]
    public GameObject expGemPrefab;

    private float currentHealth;
    private float currentSpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Transform player;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Tăng chỉ số nếu Player sống càng lâu
        float minutesPlayed = Time.timeSinceLevelLoad / 60f;
        currentHealth = health + (healthBonusPerMinute * minutesPlayed);
        currentSpeed = speed + (speedBonusPerMinute * minutesPlayed);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        // 1. TỰ ĐỘNG LẬT MẶT (Đã sửa lại cho khớp với ảnh gốc quay sang phải)
        if (player.position.x < transform.position.x)
            sr.flipX = true;  // Player ở bên trái -> Lật ảnh sang trái
        else
            sr.flipX = false; // Player ở bên phải -> Giữ nguyên ảnh gốc

        // 2. Đứng hình tạm thời khi quái bị Knockback
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            if (anim != null) anim.SetBool("Run", false); // Tắt animation chạy
            return;
        }

        // 3. Di chuyển & Ra đòn
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Rượt đuổi Player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * currentSpeed;

            // Bật hoạt ảnh chạy
            if (anim != null) anim.SetBool("Run", true);
        }
        else
        {
            // Đã áp sát -> Dừng lại
            rb.linearVelocity = Vector2.zero;

            // Tắt hoạt ảnh chạy
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
        if (anim != null)
        {
            // Random tung 1 trong 2 đòn đánh (0 hoặc 1)
            int randomAttack = Random.Range(0, 2);

            if (randomAttack == 0)
            {
                anim.SetTrigger("Attack");  // Đòn chém ngang
                KnockbackPlayer();          // Gọi hàm hất văng Player
            }
            else
            {
                anim.SetTrigger("Attack1"); // Đòn bổ củi (chỉ trừ máu, không hất)
            }
        }

        // Trừ máu Player
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
    }

    // --- HÀM HẤT VĂNG PLAYER ---
    void KnockbackPlayer()
    {
        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Tính toán hướng đẩy lùi (Từ quái hướng tới Player)
                Vector2 knockbackDirection = (player.position - transform.position).normalized;

                // Khựng Player lại một nhịp rồi mới hất đi để tạo cảm giác bị tác động mạnh
                playerRb.linearVelocity = Vector2.zero;

                // Ép lực hất văng
                playerRb.AddForce(knockbackDirection * playerKnockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    // --- NHẬN SÁT THƯƠNG TỪ PLAYER ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Dính đạn
        BulletScript dan = collision.GetComponent<BulletScript>();
        if (dan == null) dan = collision.GetComponentInParent<BulletScript>();

        if (dan != null)
        {
            ApplyKnockback(dan.transform.position);
            TakeDamage(1);
            Destroy(collision.gameObject);
            return;
        }

        // Dính kiếm xoay
        SwordOrbit kiemXoay = collision.GetComponent<SwordOrbit>();
        if (kiemXoay == null) kiemXoay = collision.GetComponentInParent<SwordOrbit>();

        if (kiemXoay != null)
        {
            ApplyKnockback(kiemXoay.transform.position);
            TakeDamage(2);
        }
    }

    // Hàm quái bị đẩy lùi
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

        rb.linearVelocity = Vector2.zero;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Rớt ngọc kinh nghiệm
        if (expGemPrefab != null)
        {
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 0.1f);
    }
}