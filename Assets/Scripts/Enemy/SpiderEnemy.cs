using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    [Header("--- Thông số cơ bản ---")]
    public float speed = 2f;
    public float health = 2f;

    [Header("--- Tiến hóa ---")]
    public float healthBonusPerMinute = 2f;
    public float speedBonusPerMinute = 0.2f;

    [Header("--- Đẩy lùi (Knockback) ---")]
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    public float stopDistance = 0.6f;

    [Header("--- Tấn công ---")]
    public int damage = 10;              // Lượng máu trừ của Player khi bị cắn
    public float attackCooldown = 1f;    // Thời gian nghỉ giữa 2 lần cắn (1 giây)
    private float nextAttackTime = 0f;

    [Header("--- Vật phẩm ---")]
    public GameObject expGemPrefab;      // Ngọc kinh nghiệm rớt ra khi chết

    // --- Biến xử lý nội bộ ---
    private float currentHealth;
    private float currentSpeed;
    private float knockbackCounter;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Tính toán chỉ số tiến hóa dựa trên thời gian game đã trôi qua
        float minutesPlayed = Time.timeSinceLevelLoad / 60f;

        currentHealth = health + (healthBonusPerMinute * minutesPlayed);
        currentSpeed = speed + (speedBonusPerMinute * minutesPlayed);

        // Tìm người chơi
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // 1. Lật mặt nhện theo hướng người chơi
        if (player.position.x > transform.position.x) sr.flipX = true;
        else sr.flipX = false;

        // 2. Logic đẩy lùi (Knockback)
        if (knockbackCounter > 0)
        {
            // Đang bị choáng do ăn đạn
            knockbackCounter -= Time.deltaTime;
            return;
        }

        // 3. Logic di chuyển và Tấn công
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Bò về phía Player
            rb.linearVelocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
        else
        {
            // Đến đủ gần (Stop Distance) -> Dừng lại cắn
            rb.linearVelocity = Vector2.zero;

            // Tấn công Player nếu đã hồi chiêu xong
            if (Time.time >= nextAttackTime)
            {
                player.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    // --- KHI NHỆN TRÚNG ĐẠN ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Tìm xem vật chạm vào có script đạn không
        BulletScript dan = collision.GetComponent<BulletScript>();
        if (dan == null) dan = collision.GetComponentInParent<BulletScript>();

        if (dan != null)
        {
            // Áp dụng Đẩy lùi (từ vị trí viên đạn)
            ApplyKnockback(dan.transform.position);

            // Trừ máu nhện (Sát thương của đạn)
            TakeDamage(1);

            // Xóa viên đạn của Player
            Destroy(collision.gameObject);
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

        // Nhện chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Rớt ngọc kinh nghiệm
        if (expGemPrefab != null)
        {
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}