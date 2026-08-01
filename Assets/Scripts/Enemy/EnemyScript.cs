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
    public GameObject damagePopupPrefab; // Kéo Prefab chữ sát thương vào đây

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        float minutesPassed = Time.timeSinceLevelLoad / 60f;
        health += Mathf.FloorToInt(minutesPassed * healthBonusPerMinute);
        speed += (minutesPassed * speedBonusPerMinute);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
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

            if (distanceToPlayer > stopDistance) rb.linearVelocity = direction * speed;
            else rb.linearVelocity = Vector2.zero;

            if (direction.x < 0) spriteRenderer.flipX = true;
            else if (direction.x > 0) spriteRenderer.flipX = false;
        }
        else rb.linearVelocity = Vector2.zero;
    }

    // Khi Kiếm xoay chém trúng
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        ShowDamagePopup(damageAmount);
        CheckDeath();
    }

    // Hàm hiển thị số sát thương bay ra
    private void ShowDamagePopup(int damageAmount)
    {
        if (damagePopupPrefab != null)
        {
            // Sinh ra chữ độc lập giữa không trung, KHÔNG dính vào quái
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
        if (health <= 0)
        {
            if (expGemPrefab != null) Instantiate(expGemPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Khi Đạn bắn trúng
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BulletScript>() != null)
        {
            int damageToTake = 1;
            if (player != null)
            {
                PlayerLevel pLevel = player.GetComponent<PlayerLevel>();
                if (pLevel != null) damageToTake = pLevel.playerDamage;
            }

            health -= damageToTake;
            ShowDamagePopup(damageToTake); // Gọi số sát thương bay ra

            knockbackCounter = knockbackTime;
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            Destroy(other.gameObject);
            CheckDeath();
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
        PlayerHealth ph = target.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(10);
    }
}