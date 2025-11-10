using System;
using Counters;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume"; 
    
    public static SoundManager Instance { get; private set; }
    
    [SerializeField]
    private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1;

    private void Awake()
    {
        Instance = this;
        
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerOnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounterOnAnyCut;
        Player.OnAnyPickedSomething += PlayerOnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounterOnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounterOnAnyObjectTrashed;
    }

    private void TrashCounterOnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounterOnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void PlayerOnPickedSomething(object sender, EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounterOnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManagerOnRecipeFailed(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverFail, DeliveryManager.Instance.transform.position);
    }

    private void DeliveryManagerOnRecipeSuccess(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryManager.Instance.transform.position);
    }

    public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
    }
    
    public  void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
    
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }
    
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }
    
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1.01f) {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float getVolume()
    {
        return volume;
    }
}
