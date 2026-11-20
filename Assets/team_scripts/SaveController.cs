using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveController : MonoBehaviour
{
    private string saveLocation;

    private void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.sceneName = SceneManager.GetActiveScene().name;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            saveData.playerPosition = player.transform.position;
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveLocation, json);
        Debug.Log("Game saved to: " + saveLocation + " (SaveController.cs)");
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            string currentSceneName = SceneManager.GetActiveScene().name;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (string.IsNullOrEmpty(DoorSpawnContext.NextSpawnId) &&
                player != null &&
                saveData.sceneName == currentSceneName)
            {
                player.transform.position = saveData.playerPosition;
            }
        }
        else
        {
            SaveGame();
        }
    }
}