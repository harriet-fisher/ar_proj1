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
    public AudioClip portalClip;

    public AudioSource finalAudio;

    public AudioSource eggSource;
    public AudioSource portalSource;

    void Start()
    {
        PlayBackgroundSound();
    }

    void Update(){
        //LogAudioSourceStatus();
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
            if (clip == doorClip){
                breathingSource.Pause();
                eggSource.clip = clip;
                eggSource.Play();
                yield return new WaitForSeconds(clip.length);
                SetEffectPitch(1.0f);
                breathingSource.UnPause();
            } else if (clip == portalClip)
            {
                portalSource.clip = clip;
                portalSource.Play();
                yield return new WaitForSeconds(clip.length);
            } else {
                breathingSource.Pause();
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length);
                breathingSource.UnPause();
            }
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

    public IEnumerator PlayPortalSound()
    {
        yield return PlaySound(portalClip);
    }

    public void EndSource(){
        breathingSource.Pause();
        audioSource.Pause();
        StartCoroutine(EndSequence());
    }

    private IEnumerator EndSequence(){
        finalAudio.loop = false;
        finalAudio.Play();
        yield return new WaitForSeconds(finalAudio.clip.length);
        finalAudio.Pause();
    }

    public void SetEffectPitch(float pitch)
    {
        if (eggSource != null)
        {
            eggSource.pitch = pitch;
        }
    }

    private void LogAudioSourceStatus()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Debug.Log("audioSource is playing: " + audioSource.clip.name);
        }
        if (breathingSource != null && breathingSource.isPlaying)
        {
            Debug.Log("breathingSource is playing: " + breathingSource.clip.name);
        }
        if (eggSource != null && eggSource.isPlaying)
        {
            Debug.Log("eggSource is playing: " + eggSource.clip.name);
        }
        if (portalSource != null && portalSource.isPlaying)
        {
            Debug.Log("portalSource is playing: " + portalSource.clip.name);
        }
        if (finalAudio != null && finalAudio.isPlaying)
        {
            Debug.Log("finalAudio is playing: " + finalAudio.clip.name);
        }
    }
}
