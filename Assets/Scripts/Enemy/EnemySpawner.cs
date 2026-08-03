using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("--- Danh sách Quái vật ---")]
    [Tooltip("Bạn có thể kéo nhiều loại quái vật vào đây")]
    public GameObject[] enemyPrefabs;

    [Header("--- Thông số đẻ quái ---")]
    public float spawnRate = 2f;    // Thời gian chờ để đẻ con tiếp theo
    public float spawnRadius = 8f;  // Bán kính khu vực đẻ quái

    private float nextSpawnTime = 0f;

    void Update()
    {
        // Kiểm tra nếu đã đến thời gian đẻ quái
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate; // Cài đặt thời gian cho lần đẻ tiếp theo
        }
    }

    void SpawnEnemy()
    {
        // Đề phòng trường hợp bạn quên kéo quái vật vào, code sẽ tự dừng để không báo lỗi
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // 1. CHỌN NGẪU NHIÊN 1 CON QUÁI VẬT TRONG DANH SÁCH
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[randomIndex];

        // 2. Tìm một vị trí ngẫu nhiên xung quanh máy sinh quái (trong phạm vi Radius)
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        // 3. Đẻ con quái ra bản đồ
        Instantiate(enemyToSpawn, randomPosition, Quaternion.identity);
    }

    // Vẽ một vòng tròn màu xanh lá cây trong cửa sổ Scene để bạn dễ dàng căn chỉnh bán kính đẻ quái
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}