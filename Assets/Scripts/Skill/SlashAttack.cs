using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [Header("--- Cài đặt Nhát Chém ---")]
    public int damage = 15;       // Sát thương của nhát chém
    public float lifeTime = 0.2f; // Thời gian tồn tại của hiệu ứng chém

    void Start()
    {
        // Tự động hủy nhát chém sau khoảng thời gian hoạt ảnh
        Destroy(gameObject, lifeTime);
    }

    // Dùng OnTriggerEnter2D để quét và gây sát thương cho TẤT CẢ quái chạm phải
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Gửi lệnh trừ máu đến mọi con quái nằm trong vùng chém
            collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

            // Debug để kiểm tra xem chém được bao nhiêu con cùng lúc
            Debug.Log("Chém trúng quái: " + collision.gameObject.name);
        }
    }
}