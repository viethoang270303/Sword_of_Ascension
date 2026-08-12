using UnityEngine;
using Unity.Cinemachine;

public class PlayerActivator : MonoBehaviour
{
    [Header("Keo dung thu tu Player1, Player2, Player3")]
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    [Header("Keo CinemachineCamera trong scene vao day")]
    public CinemachineCamera vcam;

    [Header("Keo diem spawn cho player vao day")]
    public Transform spawnPoint;

    void Awake()
    {
        int index = GameSession.SelectedCharacterIndex;

        if (player1 != null) player1.SetActive(index == 0);
        if (player2 != null) player2.SetActive(index == 1);
        if (player3 != null) player3.SetActive(index == 2);

        GameObject activePlayer = index switch
        {
            0 => player1,
            1 => player2,
            2 => player3,
            _ => player1
        };

        if (activePlayer != null && spawnPoint != null)
            activePlayer.transform.position = spawnPoint.position;

        if (vcam == null)
            vcam = FindFirstObjectByType<CinemachineCamera>();

        if (vcam != null && activePlayer != null)
            vcam.Follow = activePlayer.transform;
    }
}