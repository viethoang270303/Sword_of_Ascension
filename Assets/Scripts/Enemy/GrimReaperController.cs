using UnityEngine;

public class GrimReaperController : MonoBehaviour
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

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // --- KHAI BÁO ANIMATOR ---
    private Animator anim;
    private bool isDead = false;
    private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        float minutesPassed = Time.timeSinceLevelLoad / 60f;
        health += Mathf.FloorToInt(minutesPassed * healthBonusPerMinute);
        speed += (minutesPassed * speedBonusPerMinute);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            anim.SetBool("isWalking", false);
            return;
        }

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 direction = (player.position - transform.position).normalized;

            if (distanceToPlayer > stopDistance)
            {
                rb.linearVelocity = direction * speed;
                anim.SetBool("isWalking", true);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("isWalking", false);
            }

            if (direction.x < 0) spriteRenderer.flipX = true;
            else if (direction.x > 0) spriteRenderer.flipX = false;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isWalking", false);
        }
    }

    // Khi Kiếm xoay chém trúng (Gọi hàm này để nhảy Popup và tính máu)
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        anim.SetTrigger("Damage");

        health -= damageAmount;
        ShowDamagePopup(damageAmount);
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
            anim.SetTrigger("Death");
            rb.linearVelocity = Vector2.zero;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            if (expGemPrefab != null) Instantiate(expGemPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject, 1.5f);
        }
    }

    // Xử lý va chạm Đạn & Kiếm xoay
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        // 1. Kịch bản khi bị ĐẠN BẮN 
        if (other.GetComponent<BulletScript>() != null)
        {
            int damageToTake = 1;
            if (player != null)
            {
                PlayerLevel pLevel = player.GetComponent<PlayerLevel>();
                if (pLevel != null) damageToTake = pLevel.playerDamage;
            }

            health -= damageToTake;
            ShowDamagePopup(damageToTake);
            anim.SetTrigger("Damage");

            knockbackCounter = knockbackTime;
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            Destroy(other.gameObject);
            CheckDeath();
        }

        // ==========================================
        // 2. KỊCH BẢN KHI BỊ KIẾM XOAY CHÉM
        // Đã sửa thành đúng tên kịch bản SwordOrbit
        // ==========================================
        else if (other.GetComponent<SwordOrbit>() != null)
        {
            // Lấy trực tiếp sát thương từ biến Damage trong SwordOrbit (nếu có biến Damage)
            // Hoặc có thể set cứng là TakeDamage(2) giống trong hình của bác.
            int damageToTake = 2;

            SwordOrbit sword = other.GetComponent<SwordOrbit>();
            // Nếu bác muốn lấy sát thương từ file SwordOrbit thì có thể bỏ comment dòng dưới:
            // damageToTake = sword.Damage; // (Đảm bảo biến Damage bên SwordOrbit là public)

            TakeDamage(damageToTake);

            // Nếu muốn chém đẩy lùi thì dùng đoạn này:
            // knockbackCounter = knockbackTime;
            // Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            // rb.linearVelocity = Vector2.zero;
            // rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
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
        if (isDead) return;

        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");
            PlayerHealth ph = target.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(10);

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void Jump()
    {
        if (anim != null) anim.SetTrigger("Jump");
    }
}