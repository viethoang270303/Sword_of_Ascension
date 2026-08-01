using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject levelUpPanel;
    public Button[] skillButtons;
    public Image[] skillIcons;

    [Header("Data & Prefab")]
    public List<SkillData> allSkills;
    public GameObject swordSkillPrefab;

    private GameObject currentSword;
    private PlayerLevel playerLevel;
    private PlayerMovement playerMove;
    private PlayerHealth playerHealth;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerLevel = p.GetComponent<PlayerLevel>();
            playerMove = p.GetComponent<PlayerMovement>();
            playerHealth = p.GetComponent<PlayerHealth>();
        }

        if (levelUpPanel != null) levelUpPanel.SetActive(false);
    }

    public void ShowLevelUpUI()
    {
        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);

        // Đã xóa hàm Random. Vòng lặp này sẽ gắn cố định Skill 1->4 vào Nút 1->4
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < allSkills.Count)
            {
                skillButtons[i].gameObject.SetActive(true);

                // BẮT BUỘC PHẢI CÓ DÒNG NÀY: Khóa biến index để tránh lỗi loạn nút (Closure bug)
                int index = i;
                SkillData chosenSkill = allSkills[index];

                // Cập nhật Icon tương ứng
                if (skillIcons.Length > index && skillIcons[index] != null)
                {
                    skillIcons[index].sprite = chosenSkill.skillIcon;
                }

                // Xóa trí nhớ cũ của nút và gán chức năng mới
                skillButtons[index].onClick.RemoveAllListeners();
                skillButtons[index].onClick.AddListener(() => ChooseSkill(chosenSkill));
            }
            else
            {
                skillButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChooseSkill(SkillData skill)
    {
        switch (skill.skillType)
        {
            case SkillType.HoiMau:
                if (playerHealth != null)
                {
                    playerHealth.currentHealth += (int)skill.value;
                    if (playerHealth.currentHealth > playerHealth.maxHealth)
                        playerHealth.currentHealth = playerHealth.maxHealth;
                    playerHealth.UpdateUI();
                }
                break;

            case SkillType.TangDameDan:
                if (playerLevel != null)
                {
                    playerLevel.playerDamage += (int)skill.value;
                }
                break;

            case SkillType.TangTocChay:
                if (playerMove != null)
                {
                    playerMove.speed += skill.value;
                }
                break;

            case SkillType.KiemXoay:
                if (currentSword == null && playerMove != null)
                {
                    currentSword = Instantiate(swordSkillPrefab, playerMove.transform.position, Quaternion.identity);
                }
                break;
        }

        // Ẩn bảng và tiếp tục game
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}