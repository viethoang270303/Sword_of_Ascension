using UnityEngine;
using UnityEngine.UI;

public class DemonBoss : MonoBehaviour
{
    [Header("--- Chỉ số của Boss ---")]
    public int maxHealth = 1000;         // Máu tối đa
    private int currentHealth;           // Máu hiện tại

    public float speed = 2f;             // Tốc độ di chuyển
    public float attackRange = 2.5f;     // Tầm đánh (Giữ ở 2.5 hoặc 3)
    public float attackCooldown = 2f;    // Thời gian nghỉ giữa 2 lần chém
    public int damageToPlayer = 50;      // Lượng sát thương gây ra cho nhân vật chính

    private float nextAttackTime = 0f;
    private bool isDead = false;

    [Header("--- Liên kết UI & Mục tiêu ---")]
    public Transform player;
    public Slider healthBar;
    public GameObject healthBarCanvas;

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D coll;

    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        // Khởi tạo thanh máu
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Radar tự động tìm người chơi (Nhân vật của bạn phải được gắn tag "Player")
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        // Tính khoảng cách từ Boss đến người chơi
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // --- HƯỚNG MẶT CỦA BOSS ---
        if (player.position.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else if (player.position.x < transform.position.x)
        {
            sr.flipX = false;
        }

        // --- LOGIC CHIẾN ĐẤU & DI CHUYỂN ---
        if (distanceToPlayer <= attackRange)
        {
            anim.SetBool("isWalking", false); // Dừng lại để chém

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            anim.SetBool("isWalking", true); // Tiếp tục chạy theo
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void Attack()
    {
        // 1. Chạy animation vung búa
        anim.SetTrigger("attack");

        // 2. Gây sát thương cho Player (gọi hàm TakeDamage trên người chơi)
        if (player != null)
        {
            player.SendMessage("TakeDamage", damageToPlayer, SendMessageOptions.DontRequireReceiver);
        }
    }

    // --- HỆ THỐNG NHẬN DIỆN ĐẠN BẮN TRÚNG ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Kiểm tra xem vật thể chạm vào Boss có mang script BulletScript không
        BulletScript dan = collision.GetComponent<BulletScript>();

        if (dan != null)
        {
            // Trừ 20 máu (Có thể thay đổi số lượng máu bị trừ tại đây)
            TakeDamage(20);

            // Phá hủy viên đạn ngay sau khi chạm vào Boss
            Destroy(collision.gameObject);
        }
    }

    // --- BOSS BỊ TRỪ MÁU ---
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        // Cập nhật thanh máu UI
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        anim.SetTrigger("hit"); // Chạy animation bị đau

        // Kiểm tra xem Boss đã chết chưa
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- BOSS BAY MÀU VÀ GỌI BẢNG VICTORY ---
    void Die()
    {
        isDead = true;
        anim.SetTrigger("die"); // Chạy animation gục ngã

        // Tắt va chạm vật lý để người chơi và đạn bay xuyên qua xác Boss
        if (rb != null) rb.simulated = false;
        if (coll != null) coll.enabled = false;

        // Ẩn thanh máu đi
        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(false);
        }

        // --- GỌI HÀM BẬT BẢNG CHIẾN THẮNG ---
        if (GameManager.instance != null)
        {
            GameManager.instance.ShowVictoryScreen();
        }

        // Tắt kịch bản này đi để Boss không hoạt động nữa
        this.enabled = false;
    }
}