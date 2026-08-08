using UnityEngine;

public class PlantShooter : MonoBehaviour
{
    [Header("--- Thông số cơ bản ---")]
    public float speed = 1.5f;
    public float health = 3f;

    [Header("--- Tiến hóa ---")]
    public float healthBonusPerMinute = 2f;
    public float speedBonusPerMinute = 0.1f;

    [Header("--- Đẩy lùi (Knockback) ---")]
    public float knockbackForce = 4f;
    public float knockbackTime = 0.2f;

    [Header("--- Tấn công Xa ---")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float stoppingDistance = 5f;

    [Header("--- Vật phẩm ---")]
    public GameObject expGemPrefab;

    private float nextFireTime;
    private float currentHealth;
    private float currentSpeed;
    private float knockbackCounter;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private Animator anim; // Thêm bộ điều khiển hoạt hình

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Tự động lấy Animator trên con cây

        float minutesPlayed = Time.timeSinceLevelLoad / 60f;
        currentHealth = health + (healthBonusPerMinute * minutesPlayed);
        currentSpeed = speed + (speedBonusPerMinute * minutesPlayed);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // 1. Lật mặt theo hướng người chơi
        if (player.position.x > transform.position.x) sr.flipX = true;
        else sr.flipX = false;

        // 2. Logic choáng do knockback
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;
            if (anim != null) anim.SetBool("isRunning", false); // Bị choáng thì ngừng chạy
            return;
        }

        // 3. Logic Di chuyển & Bắn
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Đi lại gần -> Bật animation Chạy
            rb.linearVelocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);

            if (anim != null) anim.SetBool("isRunning", true);
        }
        else
        {
            // Đứng lại để bắn -> Tắt animation chạy (Về trạng thái IDL)
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("isRunning", false);

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        // Gọi hiệu ứng há mồm khạc đạn
        if (anim != null) anim.SetTrigger("Shoot");

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletScript dan = collision.GetComponent<BulletScript>();
        if (dan == null) dan = collision.GetComponentInParent<BulletScript>();

        if (dan != null)
        {
            ApplyKnockback(dan.transform.position);
            TakeDamage(1);
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

        // Bật animation nhấp nháy/bị thương
        if (anim != null) anim.SetTrigger("Hit");

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (expGemPrefab != null) Instantiate(expGemPrefab, transform.position, Quaternion.identity);

        // Tạm thời cho biến mất luôn. Nếu muốn làm animation chết từ từ thì mình sẽ độ thêm code sau nhé!
        Destroy(gameObject);
    }
}