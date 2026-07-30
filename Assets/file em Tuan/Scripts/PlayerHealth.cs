using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    [Header("Thời gian bất tử sau khi bị đánh")]
    public float invincibilityDuration = 0.5f;
    private float nextDamageTime; // Biến đếm thời gian

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.maxValue = maxHealth;

        anim = GetComponent<Animator>();
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        // 1. Nếu đã chết thì không tính nữa
        if (currentHealth <= 0) return;

        // 2. Nếu chưa hết thời gian bất tử thì KHÔNG nhận sát thương
        if (Time.time < nextDamageTime) return;

        // 3. Trừ máu và cài đặt thời gian bất tử cho lần bị đánh tiếp theo
        currentHealth -= damage;
        nextDamageTime = Time.time + invincibilityDuration;

        UpdateUI();

        if (currentHealth <= 0) Die();
    }

    public void UpdateUI()
    {
        if (healthBar != null) healthBar.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Game Over!");

        if (anim != null) anim.SetTrigger("Die");

        GetComponent<PlayerMovement>().enabled = false;

        PlayerShoot shootScript = GetComponent<PlayerShoot>();
        if (shootScript != null) shootScript.enabled = false;

        GetComponent<Collider2D>().enabled = false;
    }
}