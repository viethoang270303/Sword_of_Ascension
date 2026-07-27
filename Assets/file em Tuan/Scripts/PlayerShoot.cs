using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Cài đặt Bắn")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;

    [Header("Cài đặt Tự động ngắm")]
    public float detectRange = 6f;
    public string enemyTag = "Enemy";

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            ShootAction();
            timer = 0f;
        }
    }

    void ShootAction()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 1. Quét tìm quái vật gần nhất
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // 2. Kịch bản ngắm bắn
        // TRƯỜNG HỢP A: Có quái nằm trong tầm nhận diện -> Ngắm thẳng vào quái
        if (nearestEnemy != null && shortestDistance <= detectRange)
        {
            Vector2 direction = nearestEnemy.transform.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
        // TRƯỜNG HỢP B: Không có quái hoặc quái ở quá xa -> Xoay nòng súng Random
        else
        {
            float randomAngle = Random.Range(0f, 360f);
            firePoint.rotation = Quaternion.Euler(0, 0, randomAngle);
        }

        // 3. Khai hỏa (Tạo viên đạn)
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    // Vẽ vòng tròn đỏ trong Unity Editor để bạn dễ căn chỉnh tầm nhìn của súng
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}