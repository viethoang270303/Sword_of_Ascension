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

    [Header("Vật phẩm rơi ra")]
    [Tooltip("Kéo hộp Prefab ExpDrop vào đây")]
    public GameObject expGemPrefab;

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
        // Nếu chạm vào viên đạn
        if (other.GetComponent<BulletScript>() != null)
        {
            // --- TÍNH TOÁN SÁT THƯƠNG NHẬN VÀO TỪ PLAYER ---
            int damageToTake = 1;
            if (player != null)
            {
                PlayerLevel pLevel = player.GetComponent<PlayerLevel>();
                if (pLevel != null)
                {
                    damageToTake = pLevel.playerDamage; // Đọc chỉ số sức mạnh của Player
                }
            }

            health -= damageToTake; // Trừ máu quái
                                    // ----------------------------------------------

            // --- KÍCH HOẠT HIỆU ỨNG ĐẨY LÙI ---
            knockbackCounter = knockbackTime;
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            // ---------------------------------

            Destroy(other.gameObject); // Hủy viên đạn

            // Nếu quái hết máu -> Chết
            if (health <= 0)
            {
                // Rớt kinh nghiệm
                if (expGemPrefab != null)
                {
                    Instantiate(expGemPrefab, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }

    // --- HỆ THỐNG GÂY SÁT THƯƠNG CHO PLAYER ---
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

    void DealDamage(GameObject target)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }
}