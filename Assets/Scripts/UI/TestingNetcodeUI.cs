using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TestingNetcodeUI : MonoBehaviour
    {
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;

        private void Awake()
        {
            startHostButton.onClick.AddListener(() =>
            {
                Debug.Log("Starting host");
                KitchenGameMultiplayer.Instance.StartHost();
                Hide();
            });
            startClientButton.onClick.AddListener(() =>
            {
                Debug.Log("Starting client");
                KitchenGameMultiplayer.Instance.StartClient();
                Hide();
            });
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    
    
    }
}
