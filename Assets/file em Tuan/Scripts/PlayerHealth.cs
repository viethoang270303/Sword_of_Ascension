using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Thông số máu")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    [Header("Hiệu ứng Sát thương")]
    [Tooltip("Kéo Prefab con số nhảy dame vào đây")]
    public GameObject damagePopupPrefab;

    [Header("Cài đặt sát thương")]
    public float invincibilityTime = 0.5f;
    private float invincibilityTimer;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Đảm bảo mỗi khi bắt đầu game, thời gian luôn chạy bình thường (tránh bị kẹt 0f từ ván trước)
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int amount)
    {
        if (invincibilityTimer > 0) return;

        currentHealth -= amount;
        invincibilityTimer = invincibilityTime;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // --- TẠO SỐ NHẢY DAME ---
        if (damagePopupPrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f), 0);
            GameObject popup = Instantiate(damagePopupPrefab, transform.position + randomOffset, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(amount);
        }
        // ------------------------

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player đã hết máu! Game Over!");

        // --- ĐÓNG BĂNG THỜI GIAN VÀ TOÀN BỘ GAMEPLAY ---
        Time.timeScale = 0f;
        // ----------------------------------------------

        Destroy(gameObject);
    }
}