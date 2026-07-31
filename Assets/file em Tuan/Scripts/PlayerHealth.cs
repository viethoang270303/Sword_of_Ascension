using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    [Header("Thời gian bất tử")]
    public float invincibilityDuration = 0.5f;
    private float nextDamageTime;

    [Header("Hiệu ứng Sát thương (Chữ nổi)")]
    public GameObject damagePopupPrefab;

    [Header("UI Game Over")]
    public GameObject gameOverPanel; // Sau này bạn tạo bảng Game Over thì kéo vào đây

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.maxValue = maxHealth;

        anim = GetComponent<Animator>();
        UpdateUI();

        // Ẩn bảng Game Over lúc mới vào game
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        if (Time.time < nextDamageTime) return;

        currentHealth -= damage;
        nextDamageTime = Time.time + invincibilityDuration;

        ShowDamagePopup(damage);

        UpdateUI();

        if (currentHealth <= 0) Die();
    }

    private void ShowDamagePopup(int damageAmount)
    {
        if (damagePopupPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            DamagePopup popupScript = popup.GetComponent<DamagePopup>();
            if (popupScript != null)
            {
                popupScript.Setup(damageAmount, true);
            }
        }
    }

    public void UpdateUI()
    {
        if (healthBar != null) healthBar.value = currentHealth;
    }

    void Die()
    {
        // 1. Kích hoạt hoạt ảnh chết
        if (anim != null) anim.SetTrigger("Die");

        // 2. Tắt các kịch bản điều khiển
        GetComponent<PlayerMovement>().enabled = false;

        PlayerShoot shootScript = GetComponent<PlayerShoot>();
        if (shootScript != null) shootScript.enabled = false;

        GetComponent<Collider2D>().enabled = false;

        // 3. Hiện bảng Game Over (nếu bạn có kéo vào)
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // 4. ĐÓNG BĂNG TOÀN BỘ GAME (Quái vật, đạn, thời gian dừng hết)
        Time.timeScale = 0f;
    }
}