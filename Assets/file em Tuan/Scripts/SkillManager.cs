using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("Giao diện Level Up")]
    public GameObject levelUpPanel;
    public Button[] skillButtons;

    // Chỉ còn giữ lại ô chứa Ảnh Icon
    public Image[] skillIcons;

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

            // Bốc ngẫu nhiên 1 kỹ năng
            int randomIndex = Random.Range(0, allSkills.Count);
            SkillData chosenSkill = allSkills[randomIndex];

            // Chỉ cập nhật hiển thị Hình Ảnh
            if (skillIcons.Length > i && skillIcons[i] != null)
            {
                skillIcons[i].sprite = chosenSkill.skillIcon;
                skillIcons[i].gameObject.SetActive(chosenSkill.skillIcon != null);
            }

            // Gán sự kiện cho nút bấm
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