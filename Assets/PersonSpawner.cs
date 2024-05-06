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

   public List<Button> uiButtons;
   private bool areButtonsVisible = true;

   public Material happyMaterial;
   public Material sadMaterial;
   public Material angryMaterial;
   public Material idleMaterial;

   private int angerLevel = 0;
   
   void Start(){
    StartCoroutine(IncrementAngerOverTime());
   }

   IEnumerator IncrementAngerOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            angerLevel += 2;
            CheckAngerLevel();
        }
    }

   void Update()
    {
        if (characterAnimator != null)
        {
            AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
            bool isIdle = stateInfo.IsName("Idle");

            if (isIdle && !areButtonsVisible)
            {
                ToggleUIButtons(true);
                areButtonsVisible = true;
            }
            else if (!isIdle && areButtonsVisible)
            {
                ToggleUIButtons(false);
                areButtonsVisible = false;
            }
            if (stateInfo.IsName("Happy") && stateInfo.normalizedTime >= 1.0f)
            {
                ResetStates();
            }
            else if (stateInfo.IsName("Sad") && stateInfo.normalizedTime >= 1.0f)
            {
                ResetStates();
            }
            else if (stateInfo.IsName("Angry") && stateInfo.normalizedTime >= 1.0f)
            {
                ResetStates();
            }
        }
    }
   
   public void SpawnAdditionalPrefab(Vector3 position, Quaternion rotation)
   {
    Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
    Quaternion objectRotation = Quaternion.Euler(0f, -180f, 0f);
    Vector3 newPosition = position + new Vector3(0f, 0.001f, 0f);
    currentCharacter = Instantiate(personPrefab, newPosition, objectRotation);
    currentCharacter.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    characterAnimator = currentCharacter.GetComponent<Animator>();
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

    public void OnHappyClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetBool("isHappy", true);
            characterAnimator.SetBool("isSad", false);
            characterAnimator.SetBool("isAngry", false);
        }
        if (characterRenderer != null && happyMaterial != null)
        {
            characterRenderer.material = happyMaterial;
        }
    }

    public void OnSadClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetBool("isHappy", false);
            characterAnimator.SetBool("isSad", true);
            characterAnimator.SetBool("isAngry", false);
        }
        if (characterRenderer != null && sadMaterial != null)
        {
            characterRenderer.material = sadMaterial;
        }
    }

    public void OnAngryClick()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetBool("isHappy", false);
            characterAnimator.SetBool("isSad", false);
            characterAnimator.SetBool("isAngry", true);
        }
        if (characterRenderer != null && angryMaterial != null)
        {
            characterRenderer.material = angryMaterial;
        }

        angerLevel += 5;
        CheckAngerLevel();
    }

    void CheckAngerLevel()
    {
        if (angerLevel >= 100)
        {
            characterAnimator.SetBool("isHappy", false);
            characterAnimator.SetBool("isSad", false);
            characterAnimator.SetBool("isAngry", false);
            characterAnimator.SetBool("returnToIdle", false);
            ToggleUIButtons(false);
            if (characterRenderer != null && angryMaterial != null)
            {
                characterRenderer.material = angryMaterial;
            }
            StartCoroutine(PerformActionSequence());
        }
    }

    IEnumerator PerformActionSequence()
    {
    characterAnimator.SetTrigger("TheEnd");
    yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);

    characterAnimator.SetTrigger("triggerAction2");
    yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);

    characterAnimator.SetTrigger("triggerAction3");
    yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);

    ResetStates();
    }

    public void ResetStates()
    {
        characterAnimator.SetBool("isHappy", false);
        characterAnimator.SetBool("isSad", false);
        characterAnimator.SetBool("isAngry", false);
        characterAnimator.SetBool("returnToIdle", true);
    }

    void ToggleUIButtons(bool visible)
    {
        foreach (Button button in uiButtons)
        {
            button.gameObject.SetActive(visible);
        }
    }
}
