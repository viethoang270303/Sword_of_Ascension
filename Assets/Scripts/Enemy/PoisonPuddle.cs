using UnityEngine;

public class PoisonPuddle : MonoBehaviour
{
    [Header("--- Cấu hình Vùng Độc ---")]
    public float lifetime = 4f;       // Vùng độc tồn tại 4 giây rồi biến mất
    public int damagePerTick = 5;     // Sát thương mỗi lần rút máu
    public float tickRate = 0.5f;     // Cứ 0.5 giây rút máu 1 lần

    private float nextTickTime;

    void Start()
    {
        // Tự động xóa vùng độc sau khi hết thời gian
        Destroy(gameObject, lifetime);
    }

    // Hàm này tự động chạy liên tục miễn là Player còn đứng trong vùng chạm
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kiểm tra xem đã đến lúc rút máu tiếp chưa
            if (Time.time >= nextTickTime)
            {
                collision.SendMessage("TakeDamage", damagePerTick, SendMessageOptions.DontRequireReceiver);
                nextTickTime = Time.time + tickRate;
            }
        }
    }
}