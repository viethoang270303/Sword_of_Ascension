using UnityEngine;

public class EnemyBullet1 : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 10;
    public float lifetime = 3f; // Tự hủy sau 3 giây nếu không trúng gì

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Hủy đạn sau vài giây để giải phóng bộ nhớ
        Destroy(gameObject, lifetime);
    }

    // Kịch bản MageEnemy sẽ gọi hàm này để truyền hướng bay
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
        // Xoay hình ảnh viên đạn hướng về phía Player
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FixedUpdate()
    {
        // Bay thẳng theo hướng đã định
        rb.linearVelocity = moveDirection * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu trúng Player
        if (collision.CompareTag("Player"))
        {
            // Trừ máu Player (đảm bảo script máu của Player tên là PlayerHealth)
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Hủy đạn khi trúng đích
            Destroy(gameObject);
        }

        // (Tùy chọn) Hủy đạn nếu trúng tường/vật cản
        // else if (collision.CompareTag("Wall")) Destroy(gameObject);
    }
}