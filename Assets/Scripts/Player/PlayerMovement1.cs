using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("--- Thông số di chuyển ---")]
    public float speed = 5f;
    private float originalSpeed;        // Lưu tốc độ gốc để phục hồi sau khi hết hiệu ứng chậm
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    [Header("--- Trạng thái làm chậm (Slow) ---")]
    private bool isSlowed = false;
    private float slowTimer = 0f;

    [Header("--- Máu và Sức khỏe ---")]
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        originalSpeed = speed; // Lưu lại tốc độ ban đầu
    }

    void Update()
    {
        // Xử lý đếm ngược thời gian bị làm chậm từ đạn độc
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                speed = originalSpeed; // Hết giờ -> Phục hồi tốc độ gốc
                isSlowed = false;
            }
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = movement * speed;

        // Lật ảnh khi quay trái/phải
        if (moveX < 0) sr.flipX = true;
        else if (moveX > 0) sr.flipX = false;

        // --- CODE ANIMATION ---
        if (anim != null)
        {
            if (movement.magnitude > 0)
            {
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }
        }
    }

    // ==========================================
    // HÀM NHẬN TÁC ĐỘNG TỪ ĐẠN HOẶC QUÁI
    // ==========================================

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player dính sát thương! Máu hiện tại: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hàm nhận hiệu ứng làm chậm được gọi từ PlantBullet
    public void ApplySlow(Vector2 slowData)
    {
        float slowFactor = slowData.x;   // Tỷ lệ giảm tốc (ví dụ: 0.5 là giảm 50%)
        float slowDuration = slowData.y; // Thời gian duy trì (ví dụ: 2 giây)

        // Nếu chưa bị làm chậm trước đó, lưu lại tốc độ hiện tại làm gốc
        if (!isSlowed)
        {
            originalSpeed = speed;
        }

        // Áp dụng tốc độ mới bị giảm và đặt lại thời gian đếm ngược
        speed = originalSpeed * slowFactor;
        slowTimer = slowDuration;
        isSlowed = true;

        Debug.Log("Player bị làm chậm tốc độ trong " + slowDuration + " giây!");
    }

    void Die()
    {
        Debug.Log("Player đã chết!");
        // Thêm logic xử lý Game Over hoặc load lại màn chơi tại đây
    }
}