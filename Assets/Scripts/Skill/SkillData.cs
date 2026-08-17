using UnityEngine;

// Định nghĩa đúng 4 loại kỹ năng bạn yêu cầu
public enum SkillType
{
    HoiMau,
    TangDameDan,
    TangTocChay,
    KiemXoay,
    TangDefense,
    GiamCooldown,
    TangLifesteal,
    TangPickupRange
}

[System.Serializable]
public class SkillData
{
    public string skillName;
    [TextArea] public string description;
    public Sprite skillIcon;
    public SkillType skillType;

    [Tooltip("Điền số lượng tăng thêm. VD: Điền 30 cho Hồi Máu, Điền 1 cho Tăng Dame")]
    public float value;
}