using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    [Header("New Scene")]
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
