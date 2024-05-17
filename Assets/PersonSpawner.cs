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
                StartCoroutine(returnIdle());
            }
        }
    }
   
   public void SpawnAdditionalPrefab(Vector3 position, Quaternion rotation)
   {
    Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
    Quaternion objectRotation = Quaternion.Euler(0f, -180f, 0f);
    Vector3 newPosition = position + new Vector3(0f, 0.015f, 0f);
    currentCharacter = Instantiate(personPrefab, newPosition, objectRotation);
    currentCharacter.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
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
        if (characterRenderer != null && happyMaterial != null)
        {
            characterRenderer.material = happyMaterial;
        }
        Debug.Log("Happy animation triggered");
    }

    public void OnSadClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("isSad 0");
        }
        if (characterRenderer != null && sadMaterial != null)
        {
            characterRenderer.material = sadMaterial;
        }
    }

    IEnumerator returnIdle(){
        yield return new WaitForSeconds(2.0f);
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("sleep");
        }
        yield return new WaitForSeconds(2.0f);
        Vector3 directionToCamera = Camera.main.transform.position - currentCharacter.transform.position;
        directionToCamera.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
        currentCharacter.transform.rotation = lookRotation;
        yield return new WaitForSeconds(8.0f);
        StartCoroutine(eggSpawner.RotateHinge());
    }

    public void OnAngryClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("isAngry 0");
        }
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
