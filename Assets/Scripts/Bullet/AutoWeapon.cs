using UnityEngine;

public class AutoWeapon : MonoBehaviour
{
    [Header("--- Cài đặt Đạn ---")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("--- Tốc độ & Số lượng ---")]
    public float fireRate = 0.5f;        // Tốc độ xả đạn (Ví dụ: 0.5 giây đẻ 1 đợt)
    public int initialBulletCount = 1;   // Số lượng đạn ban đầu

    [Header("--- Tăng cấp theo thời gian ---")]
    public float timeToUpgrade = 10f;    // Cứ mỗi 10 giây sẽ bắn thêm 1 viên mỗi đợt
    public int maxBullets = 15;          // Tối đa xả 15 viên random cùng lúc cho đã

    private float fireTimer = 0f;
    private float upgradeTimer = 0f;
    private int currentBulletCount;

    void Start()
    {
        currentBulletCount = initialBulletCount;
    }

    void Update()
    {
        // 1. HỆ THỐNG ĐẺ ĐẠN LIÊN TỤC
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            ShootRandom();
            fireTimer = fireRate;
        }

        // 2. HỆ THỐNG TỰ ĐỘNG TĂNG SỐ LƯỢNG ĐẠN
        if (currentBulletCount < maxBullets)
        {
            upgradeTimer += Time.deltaTime;
            if (upgradeTimer >= timeToUpgrade)
            {
                currentBulletCount++;
                upgradeTimer = 0f;

                Debug.Log("VŨ KHÍ NÂNG CẤP! Đang bắn: " + currentBulletCount + " viên ngẫu nhiên!");
            }
        }
    }

    // --- HÀM BẮN ĐẠN RANDOM 360 ĐỘ ---
    void ShootRandom()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Đẻ đạn dựa theo số lượng đạn hiện tại
        for (int i = 0; i < currentBulletCount; i++)
        {
            // 1. Chọn một góc ngẫu nhiên từ 0 đến 360 độ
            float randomAngle = Random.Range(0f, 360f);

            // 2. Chuyển đổi góc số thành dạng xoay (Quaternion) trong Unity xoay quanh trục Z (2D)
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle);

            // 3. Đẻ viên đạn ra và ép nó phải xoay theo cái góc ngẫu nhiên vừa tạo
            Instantiate(bulletPrefab, firePoint.position, randomRotation);
        }
    }
}