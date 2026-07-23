using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Cài đặt Bắn Tự động")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Tooltip("Thời gian giữa 2 lần bắn (giây)")]
    public float fireRate = 0.5f;

    [Header("Target")]
    public float detectRange = 8f;
    public LayerMask enemyLayer;   // Chọn layer Enemy trong Inspector

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        Transform target = FindNearestEnemy();

        if (target != null)
        {
            // Có quái -> ngắm vào quái
            Vector2 dir = (target.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Không có quái -> bắn ngẫu nhiên như code cũ
            float randomAngle = Random.Range(0f, 360f);
            firePoint.rotation = Quaternion.Euler(0, 0, randomAngle);
        }

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectRange, enemyLayer);

        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}