using UnityEngine;

public class MageEnemy : MonoBehaviour
{
    [Header("--- Thông số di chuyển ---")]
    public float speed = 2.0f;
    public float retreatDistance = 3f;  // Player lại quá gần thì lùi
    public float stopDistance = 5f;     // Khoảng cách đứng lại xả đạn

    [Header("--- Thông số chiến đấu ---")]
    public float health = 8f;
    public float fireRate = 1.5f;       // Thời gian giữa các lần bắn
    private float nextFireTime;

    [Header("--- Đẩy lùi (Knockback) ---")]
    public float knockbackForce = 4f;
    public float knockbackTime = 0.2f;
    private float knockbackCounter;

    [Header("--- Bắn đạn & Rớt đồ ---")]
    public GameObject bulletPrefab;     // Kéo Prefab đạn vào đây
    public Transform firePoint;         // Vị trí gậy/tay để đạn bay ra
    public GameObject expGemPrefab;     // Ngọc kinh nghiệm

    private float currentHealth;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = health;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        // 1. LẬT MẶT THEO PLAYER (Lật ảnh SpriteRenderer)
        if (player.position.x < transform.position.x)
            sr.flipX = false; // Đổi thành true nếu ảnh gốc bị ngược hướng
        else
            sr.flipX = true;  // Đổi thành false nếu ảnh gốc bị ngược hướng

        // 2. XỬ LÝ ĐẨY LÙI KHI TRÚNG ĐẠN/KIẾM
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            return;
        }

        // 3. DI CHUYỂN GIỮ KHOẢNG CÁCH THÔNG MINH
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Tiến lại gần Player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        else if (distanceToPlayer < retreatDistance)
        {
            // Player áp sát -> Lùi lại thả diều
            Vector2 direction = (transform.position - player.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            // Đứng yên bắn
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector2 shootDirection = (player.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // GỌI ĐÚNG TÊN FILE SCRIPT ĐẠN (EnemyBullet1) ĐỂ KHÔNG BỊ LỖI ĐỎ
            EnemyBullet1 bulletScript = bullet.GetComponent<EnemyBullet1>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(shootDirection);
            }
        }
    }

    // --- VA CHẠM VÀ NHẬN SÁT THƯƠNG ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Trúng đạn Player
        BulletScript dan = collision.GetComponent<BulletScript>();
        if (dan == null) dan = collision.GetComponentInParent<BulletScript>();

        if (dan != null)
        {
            ApplyKnockback(dan.transform.position);
            TakeDamage(1);
            Destroy(collision.gameObject);
            return;
        }

        // Trúng Kiếm xoay
        SwordOrbit kiemXoay = collision.GetComponent<SwordOrbit>();
        if (kiemXoay == null) kiemXoay = collision.GetComponentInParent<SwordOrbit>();

        if (kiemXoay != null)
        {
            ApplyKnockback(kiemXoay.transform.position);
            TakeDamage(2);
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
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (expGemPrefab != null) Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f);
    }
}