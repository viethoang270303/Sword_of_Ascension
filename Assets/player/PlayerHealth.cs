using UnityEngine;
using UnityEngine.UI; // Bắt buộc phải có dòng này để dùng Slider thanh máu

public class PlayerHealth : MonoBehaviour
{
    [Header("Thông số máu")]
    public int maxHealth = 100;
    public int currentHealth;

    [Tooltip("Kéo Slider thanh máu vào đây")]
    public Slider healthBar;

    [Header("Cài đặt sát thương")]
    [Tooltip("Thời gian bất tử sau khi bị chạm (giây)")]
    public float invincibilityTime = 0.5f;
    private float invincibilityTimer;

    void Start()
    {
        // Khởi tạo máu đầy khi bắt đầu game
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        // Bộ đếm ngược thời gian bất tử
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    // Hàm này sẽ được quái vật gọi khi chúng chạm vào Player
    public void TakeDamage(int amount)
    {
        // Nếu đang trong thời gian bất tử thì bỏ qua, không trừ máu
        if (invincibilityTimer > 0) return;

        // Trừ máu và cập nhật thanh máu
        currentHealth -= amount;
        invincibilityTimer = invincibilityTime; // Bắt đầu thời gian bất tử

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Kiểm tra xem đã chết chưa
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player đã hết máu!");
        // Tạm thời xóa nhân vật khi chết. Sau này bạn có thể gọi màn hình Game Over ở đây.
        Destroy(gameObject);
    }
}