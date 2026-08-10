using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    [Header("--- Thông số cơ bản ---")]
    public float speed = 5f;
    public int damage = 10;
    public float lifeTime = 3f; // Tự hủy sau 3 giây để tránh nặng máy

    [Header("--- Hiệu ứng làm chậm (Slow) ---")]
    public float slowFactor = 0.5f;     // Giảm tốc độ nhân vật còn 50% (0.5 = giảm nửa tốc độ)
    public float slowDuration = 2f;     // Thời gian bị làm chậm trong 2 giây

    private Rigidbody2D rb;
    private bool isExploding = false;
    private Animator anim;
    private Collider2D col;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // Hủy viên đạn sau thời gian lifeTime nếu không trúng ai
        Destroy(gameObject, lifeTime);

        // Khóa hướng bay về phía vị trí của Player ngay khi xuất hiện
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            moveDirection = (player.transform.position - transform.position).normalized;

            // Xoay đầu viên đạn hướng thẳng về phía Player
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            moveDirection = Vector2.right; // Hướng mặc định nếu mất dấu Player
        }
    }

    void FixedUpdate()
    {
        // Nếu đạn đã chạm người chơi và đang nổ thì dừng di chuyển lại
        if (isExploding) return;

        // Dùng Rigidbody2D để đẩy đạn bay liên tục mỗi khung hình vật lý
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isExploding)
        {
            // 1. Gây sát thương cho Player
            collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

            // 2. Gây hiệu ứng làm chậm tốc độ di chuyển của Player (Gửi qua hàm ApplySlow)
            // Đóng gói 2 thông số (Tỷ lệ giảm tốc và Thời gian tác dụng) thành Vector2 để gửi
            collision.SendMessage("ApplySlow", new Vector2(slowFactor, slowDuration), SendMessageOptions.DontRequireReceiver);

            isExploding = true;

            // 3. Dừng đạn lại và tắt va chạm để chuẩn bị nổ
            if (rb != null) rb.linearVelocity = Vector2.zero;
            if (col != null) col.enabled = false;

            // 4. Gọi hiệu ứng Animation nổ (Trigger tên là Explode)
            if (anim != null) anim.SetTrigger("Explode");

            // 5. Chờ 0.4 giây cho animation nổ chạy xong rồi mới xóa hẳn viên đạn
            Destroy(gameObject, 0.4f);
        }
    }
}