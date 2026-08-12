using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("--- Dữ liệu Thống Kê ---")]
    public int totalKills = 0;
    public Text killText;

    [Header("--- HỆ THỐNG GỌI BOSS ---")]
    public int killsToSpawnBoss = 50;
    public GameObject bossPrefab;
    private bool isBossSpawned = false;

    [Header("--- Giao diện UI ---")]
    public GameObject victoryPanel;
    public GameObject missionPanel; // <-- BẢNG NHIỆM VỤ MỚI 
    public string mainMenuSceneName = "Main Menu";

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        UpdateUI();

        // HIỆN BẢNG NHIỆM VỤ NGAY KHI VÀO GAME
        if (missionPanel != null)
        {
            missionPanel.SetActive(true);
            Time.timeScale = 0f; // Đóng băng thời gian
        }
    }

    // ==========================================
    // HÀM GẮN VÀO NÚT "BẮT ĐẦU" TRÊN BẢNG NHIỆM VỤ
    // ==========================================
    public void StartMission()
    {
        if (missionPanel != null)
        {
            missionPanel.SetActive(false); // Ẩn bảng nhiệm vụ đi
        }
        Time.timeScale = 1f; // Rã đông thời gian, game bắt đầu!
    }

    // ==========================================
    // CÁC HỆ THỐNG CŨ GIỮ NGUYÊN
    // ==========================================
    public void AddKill()
    {
        totalKills++;
        UpdateUI();

        if (totalKills >= killsToSpawnBoss && !isBossSpawned)
        {
            SpawnBoss();
        }
    }

    void UpdateUI()
    {
        if (killText != null) killText.text = "Quái đã diệt: " + totalKills;
    }

    void SpawnBoss()
    {
        isBossSpawned = true;

        if (bossPrefab != null)
        {
            Vector3 spawnPosition = Vector3.zero;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) spawnPosition = player.transform.position + new Vector3(8f, 8f, 0);
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}