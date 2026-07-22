using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Cài đặt sinh quái")]
    public GameObject enemyPrefab; // Kéo Prefab quái vật vào đây
    public float spawnRate = 2f;   // Thời gian sinh ra 1 con (giây)
    public float spawnRadius = 8f; // Khoảng cách sinh quái so với Player

    private float timer;
    private Transform player;

    void Start()
    {
        // Quét tìm Player để lấy vị trí làm tâm điểm sinh quái
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return; // Nếu Player chết, ngừng sinh quái

        // Bộ đếm thời gian
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnEnemy();
            timer = 0f; // Đặt lại bộ đếm
        }
    }

    void SpawnEnemy()
    {
        // 1. Tạo một góc ngẫu nhiên từ 0 đến 360 độ (để quái xuất hiện tứ phía)
        float randomAngle = Random.Range(0f, 360f);

        // 2. Chuyển đổi góc đó thành hướng di chuyển (Vector2)
        // Toán học lượng giác đơn giản để xác định một điểm trên đường tròn
        Vector2 spawnDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;

        // 3. Tính toán vị trí sinh ra = Vị trí Player + (Hướng x Khoảng cách)
        Vector2 spawnPosition = (Vector2)player.position + spawnDirection * spawnRadius;

        // 4. Sinh ra quái vật tại vị trí vừa tính toán
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}