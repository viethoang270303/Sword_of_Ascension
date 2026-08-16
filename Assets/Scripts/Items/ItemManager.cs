using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Danh sách tất cả Item")]
    public List<ItemData> items = new List<ItemData>();

    public ItemData GetRandomItem()
    {
        if (items.Count == 0)
        {
            Debug.LogWarning("ItemManager chưa có Item!");
            return null;
        }

        int randomIndex = Random.Range(0, items.Count);
        return items[randomIndex];
    }

    public void GetTwoRandomItems(out ItemData item1, out ItemData item2)
    {
        item1 = null;
        item2 = null;

        if (items.Count < 2)
        {
            Debug.LogWarning("Cần ít nhất 2 Item!");
            return;
        }

        int index1 = Random.Range(0, items.Count);

        int index2;
        do
        {
            index2 = Random.Range(0, items.Count);
        }
        while (index2 == index1);

        item1 = items[index1];
        item2 = items[index2];
    }
}