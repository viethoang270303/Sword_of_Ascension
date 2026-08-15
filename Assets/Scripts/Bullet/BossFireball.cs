using UnityEngine;

public class BossFireball : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    public float lifeTime = 3f; // Sống 3 giây tự hủy nếu bay trượt

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Tìm vị trí Player lúc vừa bắn ra để nhắm tới
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * speed; // Bay thẳng về phía người chơi

            // Xoay đầu quả cầu lửa hướng theo đường bay (nếu cầu lửa có đuôi)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Bác có thể gọi hàm trừ máu Player của bác ở đây, ví dụ:
            // collision.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject); // Chạm trúng thì nổ
        }
    }
}