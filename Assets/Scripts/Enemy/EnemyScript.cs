using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Thông số cơ bản")]
    public float speed = 2f;
    public int health = 3;

    [Header("Tiến hóa")]
    public int healthBonusPerMinute = 2;
    public float speedBonusPerMinute = 0.2f;

    [Header("Đẩy lùi (Knockback)")]
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    private float knockbackCounter;
    public float stopDistance = 0.6f;

    [Header("Hiệu ứng & Vật phẩm")]
    public GameObject expGemPrefab;
    public GameObject damagePopupPrefab;

    // --- MỚI THÊM: Nút sửa lỗi đi lùi cho từng con quái ---
    [Header("Cài đặt Hình ảnh")]
    [Tooltip("Tích vào đây nếu con quái này bị lỗi đi lùi (moonwalk)")]
    public bool latNguocHinhAnh = false;

    private Transform player;
    private PlayerHealth playerHealthRef;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        float minutesPassed = Time.timeSinceLevelLoad / 60f;
        health += Mathf.FloorToInt(minutesPassed * healthBonusPerMinute);
        speed += (minutesPassed * speedBonusPerMinute);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealthRef = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            return;
        }

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 direction = (player.position - transform.position).normalized;

            if (distanceToPlayer > stopDistance) rb.linearVelocity = direction * speed;
            else rb.linearVelocity = Vector2.zero;

            // --- XỬ LÝ LẬT HÌNH THÔNG MINH ---
            if (direction.x < 0)
            {
                spriteRenderer.flipX = latNguocHinhAnh ? false : true;
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = latNguocHinhAnh ? true : false;
            }
        }
        else rb.linearVelocity = Vector2.zero;
    }

    // Khi Kiếm xoay chém trúng
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        ShowDamagePopup(damageAmount);

        if (playerHealthRef != null) playerHealthRef.ApplyLifesteal(damageAmount);

        CheckDeath();
    }

    // Hàm hiển thị số sát thương bay ra
    private void ShowDamagePopup(int damageAmount)
    {
        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
            DamagePopup popupScript = popup.GetComponent<DamagePopup>();
            if (popupScript != null)
            {
                popupScript.Setup(damageAmount);
            }
        }
    }

    private void CheckDeath()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            if (expGemPrefab != null) Instantiate(expGemPrefab, transform.position, Quaternion.identity);

            // Báo cáo về tổng đài để cộng điểm
            if (GameManager.instance != null)
            {
                GameManager.instance.AddKill();
            }

            Destroy(gameObject);
        }
    }

    // Khi Đạn bắn trúng
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        BulletScript bullet = other.GetComponent<BulletScript>();

        if (bullet == null)
            return;

        int damageToTake = 1;

        if (player != null)
        {
            PlayerLevel pLevel = player.GetComponent<PlayerLevel>();

            if (pLevel != null)
                damageToTake = pLevel.playerDamage;
        }

        // Trừ máu Enemy
        health -= damageToTake;

        // Hiện damage
        ShowDamagePopup(damageToTake);

        // Hút máu cho Player
        if (playerHealthRef != null)
            playerHealthRef.ApplyLifesteal(damageToTake);

        // Knockback
        knockbackCounter = knockbackTime;

        Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(
                knockbackDirection * knockbackForce,
                ForceMode2D.Impulse
            );
        }

        // Hủy đạn Player
        Destroy(other.gameObject);

        CheckDeath();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        if (collision.gameObject.CompareTag("Player")) DealDamage(collision.gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;
        if (collision.gameObject.CompareTag("Player")) DealDamage(collision.gameObject);
    }

    void DealDamage(GameObject target)
    {
        PlayerHealth ph = target.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(10);
    }
}