using UnityEngine;
using UnityEngine.UI;

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

    public void TakeDamage(int amount)
    {
        // Nếu đang trong thời gian bất tử thì bỏ qua
        if (invincibilityTimer > 0) return;

        // Trừ máu và cập nhật thanh UI
        currentHealth -= amount;
        invincibilityTimer = invincibilityTime;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player đã hết máu!");
        Destroy(gameObject);
    }
}