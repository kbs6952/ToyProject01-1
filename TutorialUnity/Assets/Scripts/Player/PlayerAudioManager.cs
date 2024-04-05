using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayFootStep()
    {
        if(!audioSource.isPlaying)
        audioSource.PlayOneShot(SoundManager.instance.footStepSFX);
    }
    public void PlayJumpSFX()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(SoundManager.instance.CandleSFX);
    }
    public void PlayerAllStop()
    {
        audioSource.Stop();
    }
}
