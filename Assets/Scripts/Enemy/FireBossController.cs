using UnityEngine;
using UnityEngine.UI;

public class FireBossController : MonoBehaviour
{
    [Header("--- Chỉ số Boss ---")]
    public int maxHealth = 100;
    private int currentHealth;
    public float moveSpeed = 2f;

    [Header("--- Giao diện (UI) ---")]
    public Slider healthBar; // Kéo Slider thanh máu vào ô này

    [Header("--- Cài đặt Bắn ---")]
    public GameObject fireballPrefab; // Kéo Prefab Cầu Lửa vào đây
    public Transform firePoint;       // Kéo cái nòng (FirePoint) vào đây
    public float fireCooldown = 3f;   // Mấy giây khạc 1 lần
    private float fireTimer;

    private Transform player;
    private Animator anim;
    private SpriteRenderer sr; // Dùng để lật mặt, không làm ngược thanh máu
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Cài đặt máu tối đa cho thanh máu UI ngay khi Boss sinh ra
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Bật animation bay từ lúc sinh ra
        anim.Play("FLYING");

        // Tìm kiếm mục tiêu (Player)
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        // 1. Bay theo đuổi Player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // 2. Lật mặt Boss (Nếu bác thấy nó lật bị ngược thì đổi true/false ở 2 dòng dưới cho nhau nhé)
        if (player.position.x > transform.position.x)
            sr.flipX = true; // Quay phải
        else if (player.position.x < transform.position.x)
            sr.flipX = false; // Quay trái

        // 3. Đếm ngược thời gian bắn đạn
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Attack();
            fireTimer = fireCooldown;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack"); // Kích hoạt hình khạc lửa

        // Tạo quả đạn tại đúng vị trí mồm (FirePoint)
        if (fireballPrefab != null && firePoint != null)
        {
            Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        }
    }

    // ===============================================
    // HỆ THỐNG NHẬN DIỆN ĐẠN BẮN TRÚNG
    // ===============================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Nhận diện đạn của Player
        BulletScript dan = collision.GetComponent<BulletScript>();

        if (dan != null)
        {
            // Trừ 20 máu (Bác có thể thay số này tùy ý)
            TakeDamage(20);

            // Phá hủy viên đạn ngay khi chạm vào Boss
            Destroy(collision.gameObject);
        }
    }

    // ===============================================
    // HÀM NHẬN SÁT THƯƠNG
    // ===============================================
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("Hurt"); // Giật mình, nháy đỏ

        // Cập nhật tụt máu trên UI
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Kiểm tra xem Boss đã hết máu chưa
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true); // Chạy hình ảnh chết

        // Ẩn thanh máu đi khi Boss chết
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        // Tắt va chạm vật lý để Player đi xuyên qua xác Boss
        GetComponent<Collider2D>().enabled = false;

        // --- HẸN GIỜ GỌI BẢNG CHIẾN THẮNG ---
        // Sau đúng 2 giây (khi Boss ngã xuống xong) thì bảng Victory mới hiện ra
        Invoke("WinGame", 2f);

        // Tự động dọn rác (hủy xác) sau 2.5 giây
        Destroy(gameObject, 2.5f);
    }

    // Hàm riêng để gọi bảng Chiến Thắng
    void WinGame()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ShowVictoryScreen();
        }
    }
}