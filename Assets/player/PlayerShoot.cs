using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Cài đặt Bắn Tự động")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Tooltip("Thời gian giữa 2 lần bắn (giây)")]
    public float fireRate = 0.5f;

    private float timer;

    void Update()
    {
        // Bộ đếm thời gian
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            ShootRandom360();
            timer = 0f; // Đặt lại bộ đếm
        }
    }

    void ShootRandom360()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 1. Tạo một góc ngẫu nhiên bất kỳ từ 0 đến 360 độ
        float randomAngle = Random.Range(0f, 360f);

        // 2. Xoay nòng súng (FirePoint) theo góc ngẫu nhiên đó
        firePoint.rotation = Quaternion.Euler(0, 0, randomAngle);

        // 3. Tạo viên đạn bay ra
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}