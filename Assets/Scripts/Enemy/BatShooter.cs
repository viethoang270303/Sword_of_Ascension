using UnityEngine;

public class BatShooter : MonoBehaviour
{
    [Header("--- Chỉ số của Dơi ---")]
    public int maxHealth = 50;
    private int currentHealth;

    public float speed = 1.5f;           // Tốc độ bay
    public float stoppingDistance = 5f;  // Tầm đứng lại để bắn (Cách người chơi 5m sẽ dừng lại)

    [Header("--- Vũ khí ---")]
    public GameObject enemyBulletPrefab; // Kéo Prefab đạn của DƠI vào đây
    public Transform firePoint;          // Vị trí đẻ đạn (miệng dơi)
    public float fireRate = 2f;          // Tốc độ bắn (2 giây bắn 1 lần)

    private float nextFireTime;
    private Transform player;
    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();

        // Tự động tìm nhân vật chính (Player phải được gắn Tag "Player")
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // --- LẬT MẶT DƠI VỀ PHÍA NGƯỜI CHƠI ---
        if (player.position.x > transform.position.x) sr.flipX = true;
        else sr.flipX = false;

        // --- LOGIC DI CHUYỂN & BẮN ---
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Nếu ở quá xa -> Bay lại gần người chơi
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            // Nếu đã vào tầm bắn -> ĐỨNG IM VÀ XẢ ĐẠN
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null)
        {
            Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    // --- HỆ THỐNG NHẬN SÁT THƯƠNG TỪ ĐẠN CỦA PLAYER ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Nhận diện script đạn của Player (Tên script là BulletScript)
        BulletScript dan = collision.GetComponent<BulletScript>();

        // Đề phòng trường hợp Box Collider nằm ở vật thể con của viên đạn
        if (dan == null)
        {
            dan = collision.GetComponentInParent<BulletScript>();
        }

        // Nếu đúng là đạn của Player chạm vào
        if (dan != null)
        {
            TakeDamage(20); // Trừ 20 máu của con dơi

            // Báo log ra màn hình Console để dễ kiểm tra
            Debug.Log("Dơi trúng đạn! Máu còn: " + currentHealth);

            // Hủy viên đạn của Player ngay lập tức
            Destroy(collision.gameObject);
        }
    }

    // --- HÀM TRỪ MÁU ---
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Dơi đã bị tiêu diệt!");
            Destroy(gameObject); // Con dơi nổ tung và biến mất
        }
    }
}