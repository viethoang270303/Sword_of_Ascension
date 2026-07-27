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

        // --- CODE MỚI: TẠO SỐ NHẢY DAME ---
        if (damagePopupPrefab != null)
        {
            // Cho số nhảy lệch ra một chút xíu để nếu quái cắn liên tục số không đè lên nhau
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f), 0);

            // Sinh ra vật thể chữ ở vị trí của Player
            GameObject popup = Instantiate(damagePopupPrefab, transform.position + randomOffset, Quaternion.identity);

            // Truyền con số sát thương vào cho chữ hiển thị
            popup.GetComponent<DamagePopup>().Setup(amount);
        }
        // ----------------------------------

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