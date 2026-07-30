using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootNearest();
            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootNearest()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = e;
            }
        }

        if (nearest != null)
        {
            Vector2 dir = (nearest.transform.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        }
    }
}