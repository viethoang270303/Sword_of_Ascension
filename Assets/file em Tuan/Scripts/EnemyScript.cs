using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Thông số quái vật")]
    public float speed = 2f;
    public int health = 3;

    [Header("Cài đặt Đẩy lùi (Knockback)")]
    [Tooltip("Lực đẩy văng quái ra sau khi trúng đạn")]
    public float knockbackForce = 5f;
    [Tooltip("Thời gian quái bị choáng, trôi về sau")]
    public float knockbackTime = 0.2f;
    private float knockbackCounter;

    [Header("Cài đặt Chống đẩy Player")]
    [Tooltip("Khoảng cách quái dừng lại để cắn (tránh đẩy người chơi)")]
    public float stopDistance = 0.6f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        // 1. NẾU ĐANG BỊ CHOÁNG VÌ TRÚNG ĐẠN -> BỊ ĐẨY LÙI VÀ KHÔNG THỂ ĐI TỚI
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.fixedDeltaTime;
            return; // Dừng code tại đây, bỏ qua lệnh chạy đuổi theo bên dưới
        }

        // 2. HỆ THỐNG ĐUỔI THEO & PHANH TỰ ĐỘNG (CHỐNG ĐẨY NGƯỜI)
        if (player != null)
        {
            // Đo đạc khoảng cách giữa quái và người
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 direction = (player.position - transform.position).normalized;

            // Nếu còn ở xa -> Chạy tới
            if (distanceToPlayer > stopDistance)
            {
                rb.linearVelocity = direction * speed;
            }
            // Nếu đã áp sát đủ gần -> Đứng im để cắn, không cố ủi tới để đẩy người chơi đi nữa
            else
            {
                rb.linearVelocity = Vector2.zero;
            }

            // Lật ảnh theo hướng
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
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
            health--;

            // --- KÍCH HOẠT HIỆU ỨNG ĐẨY LÙI ---
            knockbackCounter = knockbackTime; // Bắt đầu thời gian choáng

            // Tính hướng văng ra: Lấy vị trí quái vật TRỪ ĐI vị trí viên đạn
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;

            // Triệt tiêu vận tốc hiện tại và tác dụng lực văng ra sau
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            // ---------------------------------

            Destroy(other.gameObject);

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamage(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamage(collision.gameObject);
        }
    }

    // Tách riêng hàm trừ máu cho gọn code
    void DealDamage(GameObject target)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }
}