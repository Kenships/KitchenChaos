using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionButton.onClick.AddListener(() => { Hide();OptionsUI.Instance.Show(Show);});
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalGamePaused += KitchenLocalGameManagerOnLocalGamePaused; 
        KitchenGameManager.Instance.OnLocalGameUnpaused += KitchenLocalGameManagerOnLocalGameUnpaused;
        
        Hide();
    }
    
    private void KitchenLocalGameManagerOnLocalGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void KitchenLocalGameManagerOnLocalGameUnpaused(object sender, EventArgs e)
    {
        Hide();
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
