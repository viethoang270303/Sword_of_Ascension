using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpItemController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject levelUpPanel;

    [Header("Item 1")]
    public Image itemIcon1;
    public TMP_Text itemName1;
    public TMP_Text itemDescription1;
    public Button itemButton1;

    [Header("Item 2")]
    public Image itemIcon2;
    public TMP_Text itemName2;
    public TMP_Text itemDescription2;
    public Button itemButton2;

    [Header("Reroll")]
    public Button rerollButton;
    public TMP_Text rerollText; // --- MỚI THÊM: Quản lý chữ trên nút ---

    [Header("Item Manager")]
    public ItemManager itemManager;

    private ItemData currentItem1;
    private ItemData currentItem2;
    private PlayerLevel playerLevel;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private PlayerPickup playerPickup;
    private AutoSlash autoSlash;

    // Số lần đã Reroll
    private int rerollCount = 0;

    // Tối đa 2 lần Reroll
    private const int maxRerolls = 2;

    private void Start()
{
    // Tìm Player
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (player != null)
    {
        playerLevel = player.GetComponent<PlayerLevel>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerShoot = player.GetComponent<PlayerShoot>();
        playerPickup = player.GetComponent<PlayerPickup>();
        autoSlash = player.GetComponent<AutoSlash>();

        Debug.Log("Đã tìm thấy Player cho hệ thống Item.");
    }
    else
    {
        Debug.LogError("Không tìm thấy Player có Tag = Player!");
    }

    if (itemButton1 != null)
        itemButton1.onClick.AddListener(ChooseItem1);

    if (itemButton2 != null)
        itemButton2.onClick.AddListener(ChooseItem2);

    if (rerollButton != null)
        rerollButton.onClick.AddListener(Reroll);

    UpdateRerollUI();
    ShowRandomItems();
}

    public void ShowRandomItems()
    {
        if (itemManager == null)
        {
            Debug.LogError("Chưa gán ItemManager!");
            return;
        }

        itemManager.GetTwoRandomItems(out currentItem1, out currentItem2);

        if (currentItem1 == null || currentItem2 == null)
            return;

        UpdateItemUI(
            currentItem1,
            itemIcon1,
            itemName1,
            itemDescription1
        );

        UpdateItemUI(
            currentItem2,
            itemIcon2,
            itemName2,
            itemDescription2
        );
    }

    private void UpdateItemUI(
        ItemData item,
        Image icon,
        TMP_Text itemName,
        TMP_Text description)
    {
        if (item == null)
            return;

        if (icon != null)
            icon.sprite = item.icon;

        if (itemName != null)
            itemName.text = item.itemName;

        if (description != null)
            description.text = item.description;
    }

    // =========================
    // HÀM MỚI: CẬP NHẬT GIAO DIỆN NÚT REROLL
    // =========================
    private void UpdateRerollUI()
    {
        // Tính số lượt còn lại
        int rollsLeft = maxRerolls - rerollCount;

        // Cập nhật chữ hiển thị
        if (rerollText != null)
        {
            rerollText.text = "Reroll (" + rollsLeft + "/" + maxRerolls + ")";
        }

        // Tự động bật/tắt nút nếu hết lượt
        if (rerollButton != null)
        {
            rerollButton.interactable = (rerollCount < maxRerolls);
        }
    }

    // =========================
    // REROLL
    // =========================

    public void Reroll()
    {
        // Đã hết lượt thì không làm gì
        if (rerollCount >= maxRerolls)
            return;

        // Tăng số lần đã dùng
        rerollCount++;

        Debug.Log("Reroll lần " + rerollCount + "/" + maxRerolls);

        // Random lại 2 item
        ShowRandomItems();

        // Cập nhật lại UI Reroll ngay lập tức (Chữ & Trạng thái nút)
        UpdateRerollUI();

        if (rerollCount >= maxRerolls)
        {
            Debug.Log("Đã hết lượt Reroll!");
        }
    }

    // =========================
    // CHỌN ITEM
    // =========================
    private void ChooseItem1()
    {
        ChooseItem(currentItem1);
    }

    private void ChooseItem2()
    {
        ChooseItem(currentItem2);
    }

private void ChooseItem(ItemData item)
{
    if (item == null)
        return;

    Debug.Log("Đã chọn item: " + item.itemName);

    // HUYỀN THIẾT
    if (item.damagePercent != 0 && playerLevel != null)
    {
        playerLevel.playerDamage = Mathf.RoundToInt(
            playerLevel.playerDamage *
            (1f + item.damagePercent)
        );

        Debug.Log(
            "Damage hiện tại: " +
            playerLevel.playerDamage
        );
    }

    // GIÀY NGỰ PHONG
    if (item.moveSpeedPercent != 0 && playerMovement != null)
    {
        playerMovement.speed *=
            (1f + item.moveSpeedPercent);

        Debug.Log(
            "Speed hiện tại: " +
            playerMovement.speed
        );
    }

    // LINH GIÁP
    if (item.defensePercent != 0 && playerHealth != null)
    {
        playerHealth.defense +=
            item.defensePercent;

        Debug.Log(
            "Defense hiện tại: " +
            playerHealth.defense
        );
    }

    // LINH CHÂU
    if (item.cooldownPercent != 0 && playerShoot != null)
    {
        playerShoot.fireRate *=
            (1f - item.cooldownPercent);

        if (playerShoot.fireRate < 0.05f)
            playerShoot.fireRate = 0.05f;

        Debug.Log(
            "Fire Rate hiện tại: " +
            playerShoot.fireRate
        );
    }

    // HUYẾT NGỌC
    if (item.lifestealPercent != 0 && playerHealth != null)
    {
        playerHealth.lifesteal +=
            item.lifestealPercent;

        Debug.Log(
            "Lifesteal hiện tại: " +
            (playerHealth.lifesteal * 100f) +
            "%"
        );
    }

    // TỤ LINH PHÙ
    if (item.pickupRangePercent != 0 && playerPickup != null)
    {
        playerPickup.IncreasePickupRange(
            item.pickupRangePercent
        );

        Debug.Log(
            "Pickup Range hiện tại: " +
            playerPickup.pickupRange
        );
    }
    //ĐAN DƯỢC - HỒI MÁU 
    if (item.healPercent != 0 && playerHealth != null)
{
    int healAmount = Mathf.RoundToInt(
        playerHealth.maxHealth * item.healPercent
    );

    playerHealth.currentHealth += healAmount;

    if (playerHealth.currentHealth > playerHealth.maxHealth)
        playerHealth.currentHealth = playerHealth.maxHealth;

    playerHealth.UpdateUI();

    Debug.Log(
        "Đã hồi " + healAmount +
        " máu. Máu hiện tại: " +
        playerHealth.currentHealth
    );
}
    // HUYẾT ĐAO
if (item.itemName == "Huyết Đao" && autoSlash != null)
{
    autoSlash.enabled = true;

    Debug.Log("Đã mở khóa Huyết Đao!");
}

    // Đóng bảng
    if (levelUpPanel != null)
        levelUpPanel.SetActive(false);

    // Cho game chạy tiếp
    Time.timeScale = 1f;
}

    // =========================
    // MỞ LEVEL UP
    // =========================

    public void OpenLevelUp()
    {
        // Reset Reroll cho lần Level Up mới
        rerollCount = 0;

        // Random 2 item mới
        ShowRandomItems();

        // Cập nhật lại giao diện Reroll (Mở lại nút & đưa chữ về 2/2)
        UpdateRerollUI();

        // Mở Panel
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
        }
    }
}