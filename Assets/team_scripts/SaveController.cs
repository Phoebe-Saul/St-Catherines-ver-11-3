using UnityEngine;
using System.IO;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

    public void SaveGame(){
        SaveData saveData = new SaveData();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            saveData.playerPosition = player.transform.position;
        }

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(saveLocation, json);
        Debug.Log("Game saved to: " + saveLocation + "(SaveController.cs)");
    }

    public void LoadGame(){
        if(File.Exists(saveLocation)){
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
        }
        else{
            SaveGame();
        }
    }
}
