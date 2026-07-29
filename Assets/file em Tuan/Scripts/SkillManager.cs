using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    [Header("Giao diện Level Up")]
    public GameObject levelUpPanel;
    public Button[] skillButtons;
    public TextMeshProUGUI[] buttonTexts;

    // --- THÊM MỚI: Mảng chứa các ô hình ảnh Icon trên UI ---
    public Image[] skillIcons;
    // --------------------------------------------------------

    [Header("Danh sách Kỹ năng trong Game")]
    public List<SkillData> allSkills;

    [Header("Kỹ năng Kiếm Xoay")]
    public GameObject swordSkillPrefab;
    private bool hasSwordSkill = false;

    private PlayerLevel playerLevel;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerLevel = playerObj.GetComponent<PlayerLevel>();
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }

        if (levelUpPanel != null) levelUpPanel.SetActive(false);
    }

    public void ShowLevelUpPanel()
    {
        Time.timeScale = 0f;

        if (levelUpPanel != null) levelUpPanel.SetActive(true);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (allSkills.Count == 0) return;

            int randomIndex = Random.Range(0, allSkills.Count);
            SkillData chosenSkill = allSkills[randomIndex];

            // 1. Hiển thị chữ
            if (buttonTexts[i] != null)
            {
                buttonTexts[i].text = "<b>" + chosenSkill.skillName + "</b>\n" + chosenSkill.description;
            }

            // 2. Hiển thị hình ảnh (Icon)
            if (skillIcons.Length > i && skillIcons[i] != null)
            {
                skillIcons[i].sprite = chosenSkill.skillIcon;

                // Nếu kỹ năng không có ảnh, tự động ẩn ô ảnh đi cho đỡ lỗi hiển thị ô trắng
                skillIcons[i].gameObject.SetActive(chosenSkill.skillIcon != null);
            }

            skillButtons[i].onClick.RemoveAllListeners();
            skillButtons[i].onClick.AddListener(() => OnSelectSkill(chosenSkill));
        }
    }

    void OnSelectSkill(SkillData skill)
    {
        switch (skill.skillType)
        {
            case SkillType.IncreaseDamage:
                if (playerLevel != null) playerLevel.playerDamage += (int)skill.value;
                break;

            case SkillType.IncreaseSpeed:
                if (playerMovement != null) playerMovement.speed += skill.value;
                break;

            case SkillType.HealHP:
                if (playerHealth != null)
                {
                    playerHealth.currentHealth += (int)skill.value;
                    if (playerHealth.currentHealth > playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                    if (playerHealth.healthBar != null) playerHealth.healthBar.value = playerHealth.currentHealth;
                }
                break;

            case SkillType.SpinningSword:
                if (!hasSwordSkill && swordSkillPrefab != null && playerMovement != null)
                {
                    Instantiate(swordSkillPrefab, playerMovement.transform.position, Quaternion.identity);
                    hasSwordSkill = true;
                }
                break;
        }

        if (levelUpPanel != null) levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}