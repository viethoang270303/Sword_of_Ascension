using UnityEngine;

public class AutoLightning : MonoBehaviour
{
    [Header("--- Cài đặt Sét Đánh ---")]
    public GameObject lightningPrefab;    // Kéo Prefab Sét vào đây
    public float strikeInterval = 0.8f;   // Thời gian giữa các lần giáng sét (càng nhỏ sét đánh càng dày)
    public int strikeCount = 3;           // Số lượng tia sét đánh ngẫu nhiên trong mỗi đợt
    public float scanRange = 8f;          // Tầm quét tìm quái xung quanh người chơi

    private float nextStrikeTime = 0f;

    void Update()
    {
        if (Time.time >= nextStrikeTime)
        {
            nextStrikeTime = Time.time + strikeInterval;
            StrikeRandomEnemies();
        }
    }

    void StrikeRandomEnemies()
    {
        // 1. Quét toàn bộ quái trong phạm vi quanh Player
        Collider2D[] allEnemies = Physics2D.OverlapCircleAll(transform.position, scanRange);

        System.Collections.Generic.List<Transform> enemyList = new System.Collections.Generic.List<Transform>();
        foreach (Collider2D col in allEnemies)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyList.Add(col.transform);
            }
        }

        // Nếu không có con quái nào quanh đó thì thôi
        if (enemyList.Count == 0) return;

        // 2. Tiến hành đánh random vào số lượng quái tùy chỉnh
        int targetsToHit = Mathf.Min(strikeCount, enemyList.Count); // Không đánh quá số lượng quái đang có

        for (int i = 0; i < targetsToHit; i++)
        {
            // Lấy ngẫu nhiên một con quái trong danh sách
            int randomIndex = Random.Range(0, enemyList.Count);
            Transform target = enemyList[randomIndex];

            // Đẻ tia sét ngay trên đầu/vị trí của con quái đó
            Instantiate(lightningPrefab, target.position, Quaternion.identity);

            // Loại bỏ con vừa bị đánh ra khỏi danh sách để đợt này không bị lặp lại cùng 1 con
            enemyList.RemoveAt(randomIndex);
        }
    }

    // Vẽ vòng tròn phạm vi quét sét trong Scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}