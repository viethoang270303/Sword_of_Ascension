using UnityEngine;
using Unity.Cinemachine;

public class CameraAutoLock : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    void LateUpdate()
    {
        if (vcam == null) return;

        // Nếu Camera chưa có mục tiêu, HOẶC mục tiêu cũ (Player 1) đang bị tắt
        if (vcam.Target.TrackingTarget == null || !vcam.Target.TrackingTarget.gameObject.activeInHierarchy)
        {
            // Tự động quét toàn bản đồ tìm xem ông "Player" nào đang sống sờ sờ
            GameObject activePlayer = GameObject.FindGameObjectWithTag("Player");

            if (activePlayer != null)
            {
                // Cập nhật luật bám đuổi chuẩn cho Unity 6 (Cinemachine 3.x)
                vcam.Follow = activePlayer.transform;

                var camTarget = vcam.Target;
                camTarget.TrackingTarget = activePlayer.transform;
                vcam.Target = camTarget;
            }
        }
    }
}