using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string scene;
    public void playGame()
    {
        SceneManager.LoadScene(scene);
    }

    public void quitGame()
    {
        Debug.Log("quit!");
        Application.Quit();
    }
}