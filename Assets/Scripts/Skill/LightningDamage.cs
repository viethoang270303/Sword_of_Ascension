using UnityEngine;

public class LightningDamage : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 0.3f;

    [Header("Tên hàm trừ máu của quái")]
    public string tenHamTruMau = "TakeDamage"; // Bác có thể sửa tên này ở ngoài Unity

    void Start()
    {
        // Tia sét xuất hiện 0.3s rồi tự biến mất
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Chỉ cần vật chạm vào có đúng Tag "Enemy"...
        if (collision.CompareTag("Enemy"))
        {
            // 2. ...Tia sét sẽ truyền sát thương thẳng vào quái mà KHÔNG CẦN biết tên Script!
            collision.gameObject.SendMessage(tenHamTruMau, damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}