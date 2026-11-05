using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static bool IsMenuOpen{get; private set;} = false;
    public GameObject menuCanvas;

    public PauseController pauseController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(PauseController.IsGamePaused){
                if(pauseController != null){
                    pauseController.Resume();
                }
            }
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }

    public void ToggleTabMenu(){
            bool newState = !menuCanvas.activeSelf;
            menuCanvas.SetActive(newState);
            IsMenuOpen = newState;
    }
}
