using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PersonSpawner : MonoBehaviour
{
   public GameObject personPrefab;
   private Animator characterAnimator;
   public Renderer characterRenderer;
   private GameObject currentCharacter;
   public GameObject portalPrefab;
   private bool areButtonsVisible = false;

   public Material happyMaterial;
   public Material sadMaterial;
   public Material angryMaterial;
   public Material idleMaterial;
   public UIManager uiManager;
   public EggSpawner eggSpawner;
   private bool isOut = false;

   private int angerLevel = 0;
   public GameObject eggInstance;
   private Vector3 eggPosition;
   
   public SoundEffectManager soundEffectManager;
   void Start(){
    StartCoroutine(IncrementAngerOverTime());
    if (eggSpawner != null){
        Debug.Log("There is an egg spawner");
    }
   }

   IEnumerator IncrementAngerOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            angerLevel += 1;
            CheckAngerLevel();
        }
    }

   void Update()
    {
        if (characterAnimator != null)
        {
            AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
            bool isIdle = stateInfo.IsName("Idle");

            if (isIdle && !areButtonsVisible && isOut)
            {
                ToggleUIButtons(true);
                areButtonsVisible = true;
            }
            else if (!isIdle && areButtonsVisible)
            {
                ToggleUIButtons(false);
                areButtonsVisible = false;
            }
            if (stateInfo.IsName("sad_turn_final")){
                StartCoroutine(eggSpawner.UnRotateHinge());
                StartCoroutine(sleep());
            }
        }
    }
   
   public void SpawnAdditionalPrefab(Vector3 position, Quaternion rotation, GameObject egg)
   {
    eggInstance = egg;
    eggPosition = position;
    Vector3 newPosition = position + new Vector3(0.0005f, 0.015f, 0f);
    Vector3 cameraDirection = Camera.main.transform.position - newPosition;
    cameraDirection.y = 0;
    Quaternion cameraRotation = Quaternion.LookRotation(cameraDirection, Vector3.up);
    currentCharacter = Instantiate(personPrefab, newPosition, cameraRotation);
    currentCharacter.transform.localScale = new Vector3(0.045f, 0.045f, 0.045f);
    characterAnimator = currentCharacter.GetComponent<Animator>();
    SpawnPortal spawnPortal = currentCharacter.GetComponent<SpawnPortal>();

    Transform bodyTransform = currentCharacter.transform.Find("body");
        if (bodyTransform != null)
        {
            characterRenderer = bodyTransform.GetComponent<Renderer>();
        }

        if (characterRenderer != null && idleMaterial != null)
        {
            characterRenderer.material = idleMaterial;
        }
   }

   public void walkOut(){
    characterAnimator.SetTrigger("walk_out_of_egg");
    isOut = true;
   }

    public void OnHappyClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("isHappy 0");
        }
        StartCoroutine(soundEffectManager.PlayWorkSound());
        if (characterRenderer != null && happyMaterial != null)
        {
            characterRenderer.material = happyMaterial;
        }
    }

    public void OnSadClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("isSad 0");
        }
        StartCoroutine(soundEffectManager.PlaySadSound());
        if (characterRenderer != null && sadMaterial != null)
        {
            characterRenderer.material = sadMaterial;
        }
    }

    IEnumerator sleep(){
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("sleep");
        }
        StartCoroutine(soundEffectManager.PlaySleepSound());
        yield return new WaitForSeconds(12.0f);
        resetPosition();
        StartCoroutine(eggSpawner.RotateHinge());
    }

    void resetPosition(){
        Vector3 newPosition = eggPosition + new Vector3(0.0005f, 0.015f, 0f);
        Vector3 cameraDirection = Camera.main.transform.position - newPosition;
        cameraDirection.y = 0;
        Quaternion cameraRotation = Quaternion.LookRotation(cameraDirection, Vector3.up);
        currentCharacter.transform.position = newPosition;
        currentCharacter.transform.rotation = cameraRotation;
    }

    public void OnAngryClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("isAngry 0");
        }
        StartCoroutine(soundEffectManager.PlayAngrySound());
        if (characterRenderer != null && angryMaterial != null)
        {
            characterRenderer.material = angryMaterial;
        }
        angerLevel += 1;
        CheckAngerLevel();
        Debug.Log("Angry animation triggered, anger level: " + angerLevel);
    }

    void CheckAngerLevel()
    {
        if (angerLevel >= 100)
        {
            characterAnimator.SetBool("isHappy", false);
            characterAnimator.SetBool("isSad", false);
            characterAnimator.SetBool("isAngry", false);
            ToggleUIButtons(false);
            if (characterRenderer != null && angryMaterial != null)
            {
                characterRenderer.material = angryMaterial;
            }
            PerformActionSequence();
        }
    }

    public void PerformActionSequence()
    {
    characterAnimator.SetTrigger("TheEnd");
    }

    void ToggleUIButtons(bool visible)
    {
        uiManager.ShowMenu(visible);
    }
}
