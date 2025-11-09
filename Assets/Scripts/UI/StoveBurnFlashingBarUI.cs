
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    
    private const string IS_FLASHING = "IsFlashing";
    [SerializeField] private StoveCounter stoveCounter;
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
        
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool isFlashing = stoveCounter.IsFried() && e.progresssNormalized >= burnShowProgressAmount;
        animator.SetBool(IS_FLASHING, isFlashing);
    }
    
    
}
