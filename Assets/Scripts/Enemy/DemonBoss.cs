using UnityEngine;

public class DemonBoss : MonoBehaviour
{
    [Header("--- Chỉ số của Boss ---")]
    public int maxHealth = 1000;         // Máu tối đa của Boss
    private int currentHealth;           // Máu hiện tại

    public float speed = 2f;             // Tốc độ đi bộ
    public float attackRange = 1.5f;     // Khoảng cách vung rìu
    public float attackCooldown = 2f;    // Thời gian nghỉ giữa 2 nhát chém

    private float nextAttackTime = 0f;
    private bool isDead = false;         // Trạng thái sống/chết

    [Header("--- Liên kết ---")]
    public Transform player;             // Mục tiêu (Nhân vật của bạn)

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D coll;

    void Start()
    {
        // Khởi tạo máu và lấy các Component trên người Boss
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        // Tự động tìm nhân vật chính nếu bạn quên kéo vào
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        // Nếu Boss đã chết hoặc không tìm thấy người chơi -> Ngừng hoạt động
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // --- HƯỚNG MẶT CỦA BOSS ---
        // Lật ảnh để Boss luôn nhìn về phía người chơi
        if (player.position.x > transform.position.x)
            sr.flipX = false;
        else if (player.position.x < transform.position.x)
            sr.flipX = true;

        // --- LOGIC RƯỢT ĐUỔI & TẤN CÔNG ---
        if (distanceToPlayer <= attackRange)
        {
            // Trong tầm đánh: Dừng đi bộ và bổ búa
            anim.SetBool("isWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            // Ngoài tầm đánh: Chạy tới chỗ người chơi
            anim.SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void Attack()
    {
        // Kích hoạt animation chém (Boss_Cleave)
        anim.SetTrigger("attack");

        // TODO: Chỗ này gọi hàm trừ máu của người chơi (Player)
        Debug.Log("Boss Demon đang bổ rìu!");
    }

    // --- HỆ THỐNG NHẬN SÁT THƯƠNG ---
    // Gọi hàm này từ vũ khí của người chơi khi chém trúng Boss
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Nếu chết rồi thì đánh không mất máu nữa

        currentHealth -= damageAmount;

        // Kích hoạt animation giật mình (Boss_Hit)
        anim.SetTrigger("hit");

        Debug.Log("Boss bị chém! Máu còn: " + currentHealth);

        // Kiểm tra xem Boss đã cạn máu chưa
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Kích hoạt animation gục ngã (Boss_Death)
        anim.SetTrigger("die");

        // Tắt trọng lực và tắt va chạm để Boss nằm hẳn xuống đất
        // Người chơi có thể đi xuyên qua xác Boss
        if (rb != null) rb.simulated = false;
        if (coll != null) coll.enabled = false;

        // Tắt bộ não AI
        this.enabled = false;

        Debug.Log("DEMON BOSS ĐÃ BỊ TIÊU DIỆT!");
    }
}