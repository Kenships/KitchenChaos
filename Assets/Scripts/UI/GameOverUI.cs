using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainButton;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManagerOnStateChange;
        Hide();
    }

    private void KitchenGameManagerOnStateChange(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver()) {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.getSuccessfulRecipesAmount().ToString();
        }
        else {
            Hide();
        }
    }
    

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
