using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayersUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChanged += KitchenGameManager_OnLocalPlayerReadyChanged;
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;
        
        Hide();
    }

    private void KitchenGameManager_OnStateChange(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void KitchenGameManager_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsLocalPlayerReady())
        {
            Show();
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
