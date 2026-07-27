using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Thông số quái vật (Cơ bản)")]
    public float speed = 2f;
    public int health = 3;

    [Header("Cài đặt Tiến hóa theo thời gian")]
    [Tooltip("Mỗi phút trôi qua, quái cộng thêm bao nhiêu máu?")]
    public int healthBonusPerMinute = 2;
    [Tooltip("Mỗi phút trôi qua, quái chạy nhanh thêm bao nhiêu?")]
    public float speedBonusPerMinute = 0.2f;

    [Header("Cài đặt Đẩy lùi (Knockback)")]
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    private float knockbackCounter;

    [Header("Cài đặt Chống đẩy Player")]
    public float stopDistance = 0.6f;

    [Header("Vật phẩm rơi ra")]
    public GameObject expGemPrefab;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // --- TÍNH NĂNG MỚI: ĐỌC ĐỒNG HỒ VÀ TỰ TIẾN HÓA ---
        // Lấy tổng số thời gian đã chơi (tính bằng giây) chia cho 60 để ra số phút
        float minutesPassed = Time.timeSinceLevelLoad / 60f;

        // Tự cộng thêm Máu và Tốc độ dựa trên số phút đã trôi qua
        health += Mathf.FloorToInt(minutesPassed * healthBonusPerMinute);
        speed += (minutesPassed * speedBonusPerMinute);
        // -------------------------------------------------

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            return;
        }

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 direction = (player.position - transform.position).normalized;

            if (distanceToPlayer > stopDistance)
            {
                rb.linearVelocity = direction * speed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }

            if (direction.x < 0) spriteRenderer.flipX = true;
            else if (direction.x > 0) spriteRenderer.flipX = false;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BulletScript>() != null)
        {
            int damageToTake = 1;
            if (player != null)
            {
                PlayerLevel pLevel = player.GetComponent<PlayerLevel>();
                if (pLevel != null)
                {
                    damageToTake = pLevel.playerDamage;
                }
            }

            health -= damageToTake;

            knockbackCounter = knockbackTime;
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            Destroy(other.gameObject);

            if (health <= 0)
            {
                if (expGemPrefab != null)
                {
                    Instantiate(expGemPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) DealDamage(collision.gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) DealDamage(collision.gameObject);
    }

    void DealDamage(GameObject target)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }
}