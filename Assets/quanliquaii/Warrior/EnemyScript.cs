using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Thông số quái vật")]
    public float speed = 2f;     // Tốc độ chạy
    public int health = 3;       // Lượng máu (chịu được 3 viên đạn)

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Lấy các thành phần có sẵn trên quái vật
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Tự động tìm nhân vật chính trên toàn bản đồ thông qua Tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        // 1. CHẠY THEO PLAYER Ở BẤT CỨ KHOẢNG CÁCH NÀO
        if (player != null)
        {
            // Tính toán hướng đi thẳng từ quái tới vị trí hiện tại của Player
            Vector2 direction = (player.position - transform.position).normalized;

            // Dùng linearVelocity để di chuyển mượt mà (chuẩn Unity 6 LTS)
            rb.linearVelocity = direction * speed;

            // 2. LẬT HOẠT ẢNH (FLIP) THEO HƯỚNG DI CHUYỂN
            // Đi sang trái -> Lật ảnh
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            // Đi sang phải -> Trả về ảnh gốc
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            // Nếu Player bị tiêu diệt, quái vật sẽ đứng im
            rb.linearVelocity = Vector2.zero;
        }
    }

    // 3. XỬ LÝ NHẬN SÁT THƯƠNG TỪ ĐẠN (Do đạn là Is Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BulletScript>() != null)
        {
            health--; // Trừ 1 máu
            Destroy(other.gameObject); // Hủy viên đạn ngay lập tức

            // Nếu hết máu -> Tiêu diệt quái
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // 4. XỬ LÝ GÂY SÁT THƯƠNG CHO PLAYER KHI CHẠM VÀO (Do cả 2 là vật rắn)
    void OnCollisionStay2D(Collision2D collision)
    {
        // Kích hoạt liên tục nếu đang cọ xát với Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lấy script PlayerHealth từ người chơi ra
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Trừ 10 máu của Player
                playerHealth.TakeDamage(10);
            }
        }
    }
}