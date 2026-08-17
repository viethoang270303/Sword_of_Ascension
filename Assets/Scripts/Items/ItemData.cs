using UnityEngine;

public enum ItemType
{
    Passive,
    Skill
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Sword of Ascension/Item")]
public class ItemData : ScriptableObject
{
    [Header("Thông tin")]
    public string itemName;

    [TextArea(2, 4)]
    public string description;

    public Sprite icon;

    public ItemType itemType;

    [Header("Chỉ số")]
    public float damagePercent;
    public float defensePercent;
    public float moveSpeedPercent;
    public float cooldownPercent;
    public float lifestealPercent;
    public float pickupRangePercent;
    public float healPercent;
}