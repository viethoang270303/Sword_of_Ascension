using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("--- Chỉ số gốc ---")]
    public int playerDamage = 1;
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    public Slider expBar;

    private SkillManager skillManager;

    [Header("--- Kho Vũ Khí (Prefab) ---")]
    public GameObject cauNuocPrefab;    // MÀN 2: Kéo Prefab Cầu Nước vào đây
    public GameObject swordPrefab;      // MÀN 1: Kéo Prefab Kiếm Xoay vào đây
    public Transform playerTransform;   // Kéo nhân vật Player vào đây (để biết chỗ đẻ vũ khí)

    void Start()
    {
        // Đã sửa lệnh cũ thành lệnh mới FindFirstObjectByType để fix cảnh báo vàng
        skillManager = Object.FindFirstObjectByType<SkillManager>();
        UpdateUI();
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToNextLevel) LevelUp();
        UpdateUI();
    }

    void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel += 50;

        if (skillManager != null) skillManager.ShowLevelUpUI();
    }

    void UpdateUI()
    {
        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }
    }

    // ==========================================
    // CÁC HÀM GẮN VÀO NÚT BẤM "CHỌN" TRÊN UI
    // ==========================================

    // Hàm gọi Cầu Nước (Dùng cho Màn 2)
    public void AddWaterOrb()
    {
        if (cauNuocPrefab != null)
        {
            // Tự động đẻ quả cầu nước tại vị trí của Player
            Transform spawnPos = (playerTransform != null) ? playerTransform : transform;
            Instantiate(cauNuocPrefab, spawnPos.position, Quaternion.identity);

            Debug.Log("Đã chọn kỹ năng: Thêm 1 Cầu Nước!");
        }

        // Rã đông thời gian để game tiếp tục chạy
        Time.timeScale = 1f;

        // Ghi chú: Nếu bảng UI chưa tự đóng, bác có thể gắn thêm lệnh đóng UI của GameManager/SkillManager vào nút bấm nhé!
    }

    // Hàm gọi Kiếm Xoay (Dùng cho Màn 1)
    public void AddSword()
    {
        if (swordPrefab != null)
        {
            Transform spawnPos = (playerTransform != null) ? playerTransform : transform;
            Instantiate(swordPrefab, spawnPos.position, Quaternion.identity);

            Debug.Log("Đã chọn kỹ năng: Thêm 1 Kiếm Xoay!");
        }

        Time.timeScale = 1f;
    }
}