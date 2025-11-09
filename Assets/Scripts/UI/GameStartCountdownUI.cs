using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";
    
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManagerOnStateChange;
    }

    private void KitchenGameManagerOnStateChange(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Update()
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
            countdownText.text = countdownNumber.ToString();
            if (countdownNumber != previousCountdownNumber) {
                previousCountdownNumber = countdownNumber;
                animator.SetTrigger(NUMBER_POPUP);
                SoundManager.Instance.PlayCountdownSound();
            }
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
