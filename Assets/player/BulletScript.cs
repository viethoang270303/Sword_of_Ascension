using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Thông số đạn")]
    public float speed = 10f;
    public float lifeTime = 2f; // Tự động xóa sau 2 giây

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Đẩy viên đạn tiến về phía trước
        rb.linearVelocity = transform.right * speed;

        // Tự hủy viên đạn để game không bị nặng
        Destroy(gameObject, lifeTime);
    }
}