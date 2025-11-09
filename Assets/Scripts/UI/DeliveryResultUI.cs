
using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;


public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successIcon;
    [SerializeField] private Sprite failedIcon;
    
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliverManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliverManagerOnRecipeFailed;
        
        gameObject.SetActive(false);
    }

    private void DeliverManagerOnRecipeFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = failedColor;
        iconImage.sprite = failedIcon;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void DeliverManagerOnRecipeSuccess(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = successColor;
        iconImage.sprite = successIcon;
        messageText.text = "DELIVERY\nSUCCESS";
    }
}
