using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private GameObject currentPlayer;

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Chỉ spawn khi scene vừa load là màn chơi thật, không phải Persistent
        if (scene.name == "Manchoi1" || scene.name == "Manchoi2")
        {
            if (currentPlayer != null) Destroy(currentPlayer); // xóa Player màn cũ nếu còn

            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            Vector3 pos = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;

            int index = GameSession.SelectedCharacterIndex;
            if (index < 0 || index >= characterPrefabs.Length) index = 0;

            currentPlayer = Instantiate(characterPrefabs[index], pos, Quaternion.identity);

            var cam = FindObjectOfType<Unity.Cinemachine.CinemachineCamera>();
            if (cam != null) { cam.Follow = currentPlayer.transform; cam.LookAt = currentPlayer.transform; }
        }
    }
}