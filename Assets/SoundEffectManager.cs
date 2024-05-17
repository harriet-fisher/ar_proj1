using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip workClip;
    public AudioClip sadClip;
    public AudioClip angryClip;
    public AudioClip sleepClip;
    public AudioClip doorClip;
    public AudioSource breathingSource;
    public AudioClip breathingClip;

    void Start()
    {
        PlayBackgroundSound();
    }

    public IEnumerator PlayWorkSound()
    {
        yield return PlaySound(workClip);
    }

    public IEnumerator PlaySadSound()
    {
        yield return PlaySound(sadClip);
    }
    
    public IEnumerator PlaySleepSound()
    {
        yield return PlaySound(sleepClip);
    }

    public IEnumerator PlayAngrySound()
    {
        yield return PlaySound(angryClip);
    }

    public IEnumerator PlayDoorSound(){
        yield return PlaySound(doorClip);
    }

    private IEnumerator PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            breathingSource.Pause();
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length);
            SetEffectPitch(1.0f);
            breathingSource.UnPause();
        }
        yield break;
    }

    public void PlayBackgroundSound()
    {
        if (breathingSource != null && breathingClip != null)
        {
            breathingSource.clip = breathingClip;
            breathingSource.loop = true;
            breathingSource.Play();
        }
    }
    public void SetEffectPitch(float pitch)
    {
        if (audioSource != null)
        {
            audioSource.pitch = pitch;
        }
    }
}
