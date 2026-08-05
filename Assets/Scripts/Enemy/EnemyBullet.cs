using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("--- Cài đặt đạn của quái ---")]
    public float speed = 5f;            // Tốc độ bay của đạn
    public int damage = 10;             // Lượng máu trừ của Player
    public float lifeTime = 3f;         // Thời gian tự hủy để giảm lag

    private Vector2 targetDirection;

    void Start()
    {
        // 1. Tự động hủy viên đạn sau vài giây nếu bay trượt
        Destroy(gameObject, lifeTime);

        // 2. Tìm vị trí người chơi ngay khi đạn vừa được đẻ ra
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Tính góc bay thẳng từ nòng súng đến người chơi
            targetDirection = (player.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        // Bay thẳng theo hướng đã khóa mục tiêu
        transform.Translate(targetDirection * speed * Time.deltaTime);
    }

    // --- XỬ LÝ VA CHẠM VÀ TRỪ MÁU PLAYER ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem có đúng là chạm vào Player không
        if (collision.CompareTag("Player"))
        {
            // Ép người chơi nhận sát thương (gọi hàm TakeDamage trên người Player)
            collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

            // Hủy viên đạn (phát nổ)
            Destroy(gameObject);
        }
        // Tùy chọn: Hủy đạn nếu trúng tường hoặc đất
        else if (collision.CompareTag("Wall") || collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}