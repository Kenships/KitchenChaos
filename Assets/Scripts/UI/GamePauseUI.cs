using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePauseUI : MonoBehaviour
{
    
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionButton;
    
    

    private void Awake()
    {
        resumeButton.onClick.AddListener(() => { KitchenGameManager.Instance.TogglePauseGame();});
        mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainMenuScene);});
        optionButton.onClick.AddListener(() => { Hide();OptionsUI.Instance.Show(Show);});
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManagerOnGamePaused; 
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManagerOnGameUnpaused;
        
        Hide();
    }

    private void KitchenGameManagerOnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManagerOnGamePaused(object sender, EventArgs e)
    {
        Show();
    }
    
    

    private void Show(){
        resumeButton.Select();
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
}
