using UnityEngine;

public enum SkillType
{
    DanRat,
    GiayGio,
    BangCuuThuong,
    SpinningSword
}

[System.Serializable]
public class SkillData
{
    public string skillName;
    [TextArea] public string description;
    public Sprite skillIcon;
    public SkillType skillType;
    public float value; // Dùng để cộng dame, tốc độ... tùy loại skill
}