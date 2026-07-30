using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Button[] skillButtons;
    public Image[] skillIcons; // Kéo các Image icon vào đây
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

        List<SkillData> randomSkills = GetRandomSkills(skillButtons.Length);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < randomSkills.Count)
            {
                skillButtons[i].gameObject.SetActive(true);
                SkillData skill = randomSkills[i];

                if (skillIcons.Length > i && skillIcons[i] != null)
                {
                    skillIcons[i].sprite = skill.skillIcon;
                }

                skillButtons[i].onClick.RemoveAllListeners();
                skillButtons[i].onClick.AddListener(() => ChooseSkill(skill));
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
            case SkillType.SpinningSword:
                if (currentSword == null) currentSword = Instantiate(swordSkillPrefab);
                break;
            case SkillType.GiayGio:
                if (playerMove != null) playerMove.speed += skill.value;
                break;
            case SkillType.BangCuuThuong:
                if (playerHealth != null)
                {
                    playerHealth.currentHealth += (int)skill.value;
                    if (playerHealth.currentHealth > playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                    playerHealth.UpdateUI();
                }
                break;
            case SkillType.DanRat:
                if (playerLevel != null) playerLevel.playerDamage += (int)skill.value;
                break;
        }

        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private List<SkillData> GetRandomSkills(int amount)
    {
        List<SkillData> available = new List<SkillData>(allSkills);
        List<SkillData> chosen = new List<SkillData>();

        while (chosen.Count < amount && available.Count > 0)
        {
            int rand = Random.Range(0, available.Count);
            chosen.Add(available[rand]);
            available.RemoveAt(rand); // Chống lặp skill
        }
        return chosen;
    }
}