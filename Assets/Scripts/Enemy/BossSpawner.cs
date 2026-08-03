using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("--- Cài đặt Triệu hồi Boss ---")]
    public GameObject demonBoss;        // Kéo vật thể con Boss vào đây

    [Tooltip("Thời gian chờ để Boss xuất hiện (Tính bằng giây). 15 phút = 900 giây")]
    public float spawnTime = 900f;

    private float timer = 0f;
    private bool isBossSpawned = false;

    void Start()
    {
        // Ngay khi vào game, hệ thống sẽ tự động TẮT (ẩn) con Boss đi
        if (demonBoss != null)
        {
            demonBoss.SetActive(false);
        }
    }

    void Update()
    {
        // Nếu Boss đã ra rồi thì không đếm thời gian nữa
        if (isBossSpawned) return;

        // Bắt đầu đếm giờ (Time.deltaTime là thời gian trôi qua giữa mỗi khung hình)
        timer += Time.deltaTime;

        // Khi đồng hồ đếm đạt mốc 15 phút (900 giây)
        if (timer >= spawnTime)
        {
            SpawnTheBoss();
        }
    }

    void SpawnTheBoss()
    {
        if (demonBoss != null)
        {
            // 1. Dịch chuyển Boss đến vị trí của điểm Spawner này
            demonBoss.transform.position = transform.position;

            // 2. BẬT con Boss lên để nó bắt đầu hoạt động và rượt đuổi
            demonBoss.SetActive(true);

            Debug.Log("CẢNH BÁO: DEMON BOSS ĐÃ XUẤT HIỆN!!!");
            isBossSpawned = true;
        }
    }
}