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

    [Header("--- Kho Vũ Khí / Item ---")]
    public GameObject ThuyCauPrefab;
    public GameObject swordPrefab;
    public GameObject autoSlashPrefab;


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

// ==========================================
// THỦY CẦU
// ==========================================
public void AddWaterOrb()
{
    if (ThuyCauPrefab != null)
    {
        Instantiate(
            ThuyCauPrefab,
            transform.position,
            Quaternion.identity
        );

        Debug.Log("Đã chọn kỹ năng: Thủy Cầu!");
    }

    Time.timeScale = 1f;
}


// ==========================================
// KIẾM XOAY
// ==========================================
public void AddSword()
{
    if (swordPrefab != null)
    {
        Instantiate(
            swordPrefab,
            transform.position,
            Quaternion.identity
        );

        Debug.Log("Đã chọn kỹ năng: Kiếm Xoay!");
    }

    Time.timeScale = 1f;
}


// ==========================================
// HUYẾT ĐAO
// ==========================================
public void AddHuyetDao()
{
    if (autoSlashPrefab != null)
    {
        Instantiate(
            autoSlashPrefab,
            transform.position,
            Quaternion.identity
        );

        Debug.Log("Đã chọn item: Huyết Đao!");
    }
    else
    {
        Debug.LogWarning("Chưa gán AutoSlash Prefab cho Player!");
    }

    Time.timeScale = 1f;
}

    // ==========================================
    // HÀM GỌI TIA SÉT (Dùng cho Player mang kịch bản Sét)
    // ==========================================
    public void AddLightning()
    {
        // Đi tìm kịch bản Sét (động cơ đẻ sét liên tục) trên người nhân vật
        AutoLightning lightningScript = GetComponent<AutoLightning>();

        if (lightningScript != null)
        {
            // Bật công tắc cho kịch bản Sét thức dậy và chạy vòng lặp đánh liên tục
            lightningScript.enabled = true;
            Debug.Log("Đã mở khóa: Sét giật liên tục!");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy kịch bản AutoLightning trên người Player này! Nhớ kéo thả vào Inspector nhé.");
        }

        // Rã đông thời gian để game chạy tiếp
        Time.timeScale = 1f;
    }
}