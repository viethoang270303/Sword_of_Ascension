using UnityEngine;

public class Player3SkillManager : MonoBehaviour
{
    // Hàm này sẽ được gọi khi bác bấm vào nút "Học Chiêu Sét" trên UI
    public void LearnLightningSkill()
    {
        // Tìm Player 3 đang đứng trên sân (Nhờ cái Tag "Player" lúc nãy)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Tìm kịch bản Sét trên người Player 3
            AutoLightning lightningSkill = player.GetComponent<AutoLightning>();

            if (lightningSkill != null)
            {
                if (!lightningSkill.enabled)
                {
                    // Lần đầu chọn chiêu: Đánh thức kịch bản dậy
                    lightningSkill.enabled = true;
                    Debug.Log("Player 3 đã mở khóa chiêu Sét!");
                }
                else
                {
                    // Nếu lên cấp cao mà chọn lại chiêu này: Nâng cấp sức mạnh!
                    // Tăng thêm 1 tia sét giáng xuống cùng lúc
                    lightningSkill.strikeCount += 1;
                    Debug.Log("Chiêu Sét đã được nâng cấp: Đánh " + lightningSkill.strikeCount + " tia!");
                }
            }
        }

        // TODO: Đoạn này bác thêm code tắt Bảng Chọn Chiêu và cho game chạy tiếp (Time.timeScale = 1) giống các nút khác của bác nhé.
    }
}