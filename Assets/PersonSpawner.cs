using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PersonSpawner : MonoBehaviour
{
   public GameObject personPrefab;
   public float resetDuration = 2.0f;
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

   public SpawnPortal spawnPortal;
   private bool incrementAnger = true;
   
   public SoundEffectManager soundEffectManager;
   void Start(){
    StartCoroutine(IncrementAngerOverTime());
    if (eggSpawner != null){
        Debug.Log("There is an egg spawner");
    }
   }

   IEnumerator IncrementAngerOverTime()
    {
        while (incrementAnger)
        {
            yield return new WaitForSeconds(1);
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
    spawnPortal.character = currentCharacter;

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
    if (!isOut){
        characterAnimator.SetTrigger("walk_out_of_egg");
        isOut = true;
    }
   }

    public void OnHappyClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("isHappy 0");
        }
        if (characterRenderer != null && happyMaterial != null)
        {
            characterRenderer.material = happyMaterial;
        }
        StartCoroutine(HappySound());
    }

    IEnumerator HappySound(){
        yield return new WaitForSeconds(10.0f);
        StartCoroutine(soundEffectManager.PlayWorkSound());
        yield return new WaitForSeconds(9.0f);
        StartCoroutine(resetPosition());
        yield return new WaitForSeconds(3.0f);
        walkOut();
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
    public void sleepTrigger(){
        Debug.Log("Entering sleepTrigger");
        StartCoroutine(eggSpawner.UnRotateHinge());
        sleep();
    }

    IEnumerator sleep(){
        Debug.Log("Entering sleep coroutine");
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("sleep");
        }
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(soundEffectManager.PlaySleepSound());
        yield return new WaitForSeconds(12.0f);
        StartCoroutine(eggSpawner.RotateHinge());
        StartCoroutine(resetPosition());
        Debug.Log("Finished waiting in sleep coroutine");
    }
    IEnumerator resetPosition(){
        isOut = false;
        Vector3 startPosition = currentCharacter.transform.position;
        Vector3 targetPosition = eggPosition + new Vector3(0.0005f, 0.015f, 0f);
        Vector3 cameraDirection = Camera.main.transform.position - targetPosition;
        cameraDirection.y = 0;
        Quaternion startRotation = currentCharacter.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(cameraDirection, Vector3.up);

        float elapsedTime = 0f;

        while (elapsedTime < resetDuration)
        {
            currentCharacter.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / resetDuration);
            currentCharacter.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / resetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentCharacter.transform.position = targetPosition;
        currentCharacter.transform.rotation = targetRotation;

        Debug.Log("Got past reset position in sleep coroutine");

        /*Vector3 newPosition = eggPosition + new Vector3(0.0005f, 0.015f, 0f);
        Debug.Log("New Position: " + newPosition);
        Vector3 cameraDirection = Camera.main.transform.position - newPosition;
        cameraDirection.y = 0;
        Quaternion cameraRotation = Quaternion.LookRotation(cameraDirection, Vector3.up);
        Debug.Log("New Rotation: " + cameraRotation);
        currentCharacter.transform.position = newPosition;
        currentCharacter.transform.rotation = cameraRotation;
        Debug.Log("Got past reset position in sleep coroutine");*/
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
            ToggleUIButtons(false);
            if (characterRenderer != null && angryMaterial != null)
            {
                characterRenderer.material = angryMaterial;
            }
            AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
            bool isIdle = stateInfo.IsName("Idle");
            if (isIdle){
                PerformActionSequence();
            }
        }
    }

    public void PerformActionSequence()
    {
    incrementAnger = false;
    angerLevel = 0;
    characterAnimator.SetTrigger("TheEnd");
    soundEffectManager.EndSource();
    }

    void ToggleUIButtons(bool visible)
    {
        uiManager.ShowMenu(visible);
    }
}
