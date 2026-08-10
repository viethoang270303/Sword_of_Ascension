using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    // Gọi khi bấm nút Player 1
    public void SelectPlayer1()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 1);
        PlayerPrefs.Save();
        Debug.Log("Đã chọn Player 1");
    }

    // Gọi khi bấm nút Player 2
    public void SelectPlayer2()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 2);
        PlayerPrefs.Save();
        Debug.Log("Đã chọn Player 2");
    }

    // Gọi khi bấm nút Player 3
    public void SelectPlayer3()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 3);
        PlayerPrefs.Save();
        Debug.Log("Đã chọn Player 3");
    }
}