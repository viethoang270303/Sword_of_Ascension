using UnityEngine;
using UnityEngine.UI;

public class ShadowDragonController : MonoBehaviour
{
    [Header("--- Chỉ số Boss ---")]
    public int maxHealth = 500;
    private int currentHealth;
    public float moveSpeed = 2.5f;
    public float attackRange = 3f;
    public float attackCooldown = 2.5f;
    public int damageToPlayer = 40;

    private float attackTimer = 0f;
    private float animLockTimer = 0f; // Bộ đếm giờ để khóa hoạt ảnh (chờ rồng cắn/đau xong)
    private bool isDead = false;

    [Header("--- UI & Mục tiêu ---")]
    public Transform player;
    public Slider healthBar;
    public GameObject healthBarCanvas;

    [Header("--- Âm thanh (Audio) ---")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D coll;
    private string currentState; // Lưu lại tên hoạt ảnh đang chạy

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    // Cỗ máy tự động chuyển hoạt ảnh bằng TÊN KHỐI
    void ChangeAnim(string newState)
    {
        if (currentState == newState) return; // Nếu đang chiếu phim này rồi thì thôi
        anim.Play(newState);                  // Bật phim mới
        currentState = newState;
    }

    void Update()
    {
        if (isDead || player == null) return;

        // Trừ lùi thời gian khóa và thời gian hồi chiêu
        animLockTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        // Xác định hướng của Player để gép đuôi chữ "_Right" hoặc "_Left"
        string huong = (player.position.x > transform.position.x) ? "_Right" : "_Left";

        // Nếu rồng đang bận cắn hoặc đang nhăn mặt kêu đau (bị khóa) -> Đứng im chờ chiếu hết phim
        if (animLockTimer > 0f) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Tới tầm cắn
            if (attackTimer <= 0f)
            {
                Attack(huong);
                attackTimer = attackCooldown;
            }
            else
            {
                // Tới tầm nhưng đang chờ hồi chiêu thì đứng gầm gừ
                ChangeAnim("Idle" + huong);
            }
        }
        else
        {
            // Chưa tới tầm thì đi bộ đuổi theo
            ChangeAnim("Walk" + huong);
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void Attack(string huong)
    {
        // Tự động gọi khối: "Attack_Left" hoặc "Attack_Right"
        ChangeAnim("Attack" + huong);

        // Khóa hoạt ảnh 0.8 giây để chờ nó diễn xong cảnh cắn (Bác có thể chỉnh số này cho khớp với độ dài của phim)
        animLockTimer = 0.8f;

        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        if (player != null)
        {
            player.SendMessage("TakeDamage", damageToPlayer, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        BulletScript dan = collision.GetComponent<BulletScript>();
        if (dan != null)
        {
            TakeDamage(25); // Đạn trúng trừ 25 máu
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        string huong = (player.position.x > transform.position.x) ? "_Right" : "_Left";
        ChangeAnim("Hurt" + huong);

        // Khóa 0.5s để rồng nhăn mặt kêu đau
        animLockTimer = 0.5f;

        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0) Die(huong);
    }

    void Die(string huong)
    {
        isDead = true;
        ChangeAnim("Death" + huong);

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (rb != null) rb.simulated = false;
        if (coll != null) coll.enabled = false;
        if (healthBarCanvas != null) healthBarCanvas.SetActive(false);

        Invoke("WinGame", 2.5f);
        Destroy(gameObject, 3f);
    }

    void WinGame()
    {
        if (GameManager.instance != null)
            GameManager.instance.ShowVictoryScreen();
    }
}