using UnityEngine;

public class PoisonRingProjectile : MonoBehaviour
{
    [Header("--- Cấu hình Đạn ---")]
    public float speed = 5f;          // Tốc độ bay của vòng độc
    public float lifeTime = 5f;       // Tự hủy sau 5 giây nếu bay trượt ra ngoài map

    [Header("--- Gắn Vùng Độc ---")]
    public GameObject poisonPuddlePrefab; // Kéo Prefab vùng độc rút máu vào đây

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool hasExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Hủy viên đạn nếu không trúng ai sau 1 khoảng thời gian (chống lag)
        Destroy(gameObject, lifeTime);

        // Khóa mục tiêu Player ngay khi vừa đẻ ra
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            moveDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            moveDirection = Vector2.right; // Hướng bay dự phòng nếu mất dấu Player
        }
    }

    void FixedUpdate()
    {
        // Nếu đã nổ rồi thì không bay nữa
        if (hasExploded) return;

        // Dùng Rigidbody2D để đẩy đạn bay liên tục
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi chạm vào người chơi
        if (collision.CompareTag("Player") && !hasExploded)
        {
            hasExploded = true;

            // 1. Sinh ra Vùng Độc (AoE rút máu) ngay tại vị trí dưới chân Player
            if (poisonPuddlePrefab != null)
            {
                Instantiate(poisonPuddlePrefab, collision.transform.position, Quaternion.identity);
            }

            // 2. Tiêu hủy viên đạn bay này đi
            Destroy(gameObject);
        }
    }
}