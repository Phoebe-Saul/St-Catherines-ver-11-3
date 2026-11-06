using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string scene;
    public void playGame()
    {
        SceneManager.LoadScene(scene);
    }

    public void CloseGame()
    {
        Debug.Log("quit!");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}