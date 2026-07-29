using UnityEngine;

public enum SkillType
{
    IncreaseDamage,
    IncreaseSpeed,
    HealHP,
    SpinningSword
}

[System.Serializable]
public class SkillData
{
    public string skillName;

    [TextArea]
    public string description;

    // --- CẬP NHẬT MỚI: Thêm ô chứa hình ảnh Icon ---
    public Sprite skillIcon;
    // -----------------------------------------------

    public SkillType skillType;
    public float value;
}