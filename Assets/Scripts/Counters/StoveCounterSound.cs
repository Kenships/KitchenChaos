
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField]
    private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private float warningSoundTimer;
    
    private bool playWarningSound;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
    }

    private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.progresssNormalized >= burnShowProgressAmount;
    }

    private void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playsound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        
        if (playsound) {
            audioSource.Play();
        }
        else {
            audioSource.Pause();
        }
    }
    
    private void Update()
    {
        if (playWarningSound) {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0) {
                float warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;
                
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
