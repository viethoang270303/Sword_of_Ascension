using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("--- Cài đặt Triệu hồi Boss ---")]
    public GameObject demonBossPrefab;  // Đã đổi tên biến để tránh nhầm lẫn
    public float spawnTime = 5f;

    private float timer = 0f;
    private bool isBossSpawned = false;

    void Update()
    {
        if (isBossSpawned) return;

        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnTheBoss();
        }
    }

    void SpawnTheBoss()
    {
        if (demonBossPrefab != null)
        {
            // LỆNH QUAN TRỌNG NHẤT: Đẻ con Boss từ kho Prefab ra đúng vị trí Spawner
            Instantiate(demonBossPrefab, transform.position, Quaternion.identity);

            // Dòng log mới để bạn phân biệt code đã cập nhật chưa
            Debug.Log("CẢNH BÁO: DEMON BOSS ĐÃ ĐƯỢC ĐẺ RA TỪ PREFAB!!!");
            isBossSpawned = true;
        }
    }
}