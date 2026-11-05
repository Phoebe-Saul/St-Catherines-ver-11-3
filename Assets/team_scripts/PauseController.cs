using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour{
    public static bool IsGamePaused{ get; private set;} = false;
    public GameObject pauseMenuUI;

    [Header("Other Menus")]
    public GameObject tabMenu;

    public static void SetPause(bool pause){
        IsGamePaused = pause;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(IsGamePaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void Pause(){
        if(tabMenu != null && tabMenu.activeSelf){
            tabMenu.SetActive(false);
        }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void LoadMenu(){
        Debug.Log("Loadmenu");
    }

    public void QuitGame(){
        Debug.Log("Quitgame");
    }
}