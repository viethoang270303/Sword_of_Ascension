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
    private bool isDead = false; // Chốt an toàn chống đếm kill 2 lần

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private Animator anim;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        float minutesPlayed = Time.timeSinceLevelLoad / 60f;
        currentHealth = health + (healthBonusPerMinute * minutesPlayed);
        currentSpeed = speed + (speedBonusPerMinute * minutesPlayed);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || isDead) return;

        // 1. Lật mặt theo hướng người chơi 
        if (player.position.x > transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;

        // 2. Logic choáng do knockback
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;
            return;
        }

        // 3. Di chuyển & Bắn
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Đi lại gần người chơi
            rb.linearVelocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
        else
        {
            // Đã vào tầm -> Đứng lại và khạc đạn
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
        if (anim != null) anim.SetTrigger("Shoot");

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

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
        if (isDead) return;
        currentHealth -= damageAmount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (expGemPrefab != null) Instantiate(expGemPrefab, transform.position, Quaternion.identity);

        // ---> BÁO CÁO VỀ TỔNG ĐÀI ĐỂ CỘNG ĐIỂM <---
        if (GameManager.instance != null)
        {
            GameManager.instance.AddKill();
        }

        Destroy(gameObject);
    }
}