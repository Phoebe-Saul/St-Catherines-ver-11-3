using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour{
    public static bool IsGamePaused{ get; private set;} = false;

    [Header("UI Panels")]
    public GameObject pauseMenuUI;
    public GameObject pauseButtonsPanel;
    public GameObject confirmationPanel;
    public GameObject settingsMessagePanel;
    public GameObject loadMessagePanel;

    [Header("Other Menus")]
    public GameObject tabMenu;



    public static void SetPause(bool pause){
        IsGamePaused = pause;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(confirmationPanel != null && confirmationPanel.activeSelf){
                HideConfirmation();
                return;
            }
            if(settingsMessagePanel != null && settingsMessagePanel.activeSelf){
                HideSettingsMessage();
                return;
            }
            if(loadMessagePanel != null && loadMessagePanel.activeSelf){
                HideLoadMessage();
                return;
            }

            if(IsGamePaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        if(confirmationPanel != null) confirmationPanel.SetActive(false);
        if(loadMessagePanel != null) loadMessagePanel.SetActive(false);
        if(settingsMessagePanel != null) settingsMessagePanel.SetActive(false);
        if(pauseButtonsPanel != null) pauseButtonsPanel.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void Pause(){
        if(tabMenu != null && tabMenu.activeSelf){
            tabMenu.SetActive(false);
        }
        if(confirmationPanel != null) confirmationPanel.SetActive(false);
        if(loadMessagePanel != null) loadMessagePanel.SetActive(false);
        if(settingsMessagePanel != null) settingsMessagePanel.SetActive(false);
        if(pauseButtonsPanel != null) pauseButtonsPanel.SetActive(true);

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void LoadMenu(){
        Debug.Log("Loadmenu");
    }

    public void PromptQuitToMenu(){
        if(pauseButtonsPanel != null){
            pauseButtonsPanel.SetActive(false); // Hides the main pause buttons
        }
        if(confirmationPanel != null){
            confirmationPanel.SetActive(true);  // Shows the confirmation prompt
        }
    }

    public void HideConfirmation(){
        if(confirmationPanel != null){
            confirmationPanel.SetActive(false); // Hides confirmation prompt
        }
        if(pauseButtonsPanel != null){
            pauseButtonsPanel.SetActive(true);  // Brings back main pause buttons
        }
    }

    public void PromptLoadMessage(){
        if(pauseButtonsPanel != null) pauseButtonsPanel.SetActive(false);
        if(loadMessagePanel != null) loadMessagePanel.SetActive(true);
    }

    public void HideLoadMessage(){
        if(loadMessagePanel != null) loadMessagePanel.SetActive(false);
        if(pauseButtonsPanel != null) pauseButtonsPanel.SetActive(true);
    }

    public void PromptSettingsMessage(){
        if(pauseButtonsPanel != null) pauseButtonsPanel.SetActive(false);
        if(settingsMessagePanel != null) settingsMessagePanel.SetActive(true);
    }

    public void HideSettingsMessage(){
        if(settingsMessagePanel != null) settingsMessagePanel.SetActive(false);
        if(pauseButtonsPanel != null) pauseButtonsPanel.SetActive(true);
    }

    public void ConfirmQuitToMenu(){
        Time.timeScale = 1f;
        IsGamePaused = false;
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame(){
        //Debug.Log("Quitgame");
        SceneManager.LoadScene("StartMenu");
    }

    
}