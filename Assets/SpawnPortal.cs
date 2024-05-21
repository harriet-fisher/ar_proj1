using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnPortal : MonoBehaviour
{
    public GameObject portalPrefab;
    private Animator animator;
    private bool hasSpawnedPortal = false;
    public float offsetY = 0.5f;
    private ARPlaneManager arPlaneManager;
    private float arSurfaceY;
    public GameObject spawnedPortal;
    private Renderer[] characterRenderers;
    public float disappearSpeed = 1.0f;
    public float collapseSpeed = 0.02f;
    public UIManager uiManager;
    public GameObject character;
    public SoundEffectManager soundEffectManager;
    private bool hasJumped = false;

    void Start()
    {
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        StartCoroutine(UpdateARSurfaceY());
    }

    // Update is called once per frame
    void Update()
    {
        if (character != null){
            animator = character.GetComponent<Animator>();
            characterRenderers = character.GetComponentsInChildren<Renderer>();
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("jump_down") && !hasSpawnedPortal)
            {
                OnJump();
            }
        }
    }

    IEnumerator UpdateARSurfaceY()
    {
        while (true)
        {
            if (arPlaneManager != null)
            {
                foreach (ARPlane plane in arPlaneManager.trackables)
                {
                    arSurfaceY = plane.transform.position.y;
                    break;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    void OnJump()
    {
        if (!hasSpawnedPortal && !hasJumped && portalPrefab != null)
        {
            hasJumped = true;
            Vector3 spawnPosition = new Vector3(character.transform.position.x-0.03f, arSurfaceY, character.transform.position.z - 0.03f);
            spawnedPortal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
            spawnedPortal.transform.localScale = new Vector3(0.000f, 0.000f, 0.000f);
            StartCoroutine(PopUpPortal());
        }
    }

    IEnumerator PopUpPortal()
    {
        while (spawnedPortal.transform.localScale.magnitude < Mathf.Sqrt(3 * 0.06f * 0.06f))
        {
            Debug.Log("Portal Size: " + spawnedPortal.transform.localScale);
            float newScaleValue = spawnedPortal.transform.localScale.x + (collapseSpeed * Time.deltaTime);
            spawnedPortal.transform.localScale = new Vector3(newScaleValue, newScaleValue, newScaleValue);
            yield return null;
        }
        Debug.Log("Portal Fully Opened: " + spawnedPortal.transform.localScale);
        StartCoroutine(soundEffectManager.PlayPortalSound());
        hasSpawnedPortal = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveDownAndDisappear());
    }
    IEnumerator MoveDownAndDisappear()
    {
        float initialY = character.transform.position.y;
        while (character.transform.position.y > arSurfaceY)
        {
            character.transform.position += Vector3.down * disappearSpeed * Time.deltaTime;
            float alpha = Mathf.Clamp01((character.transform.position.y - (arSurfaceY - offsetY)) / (initialY - (arSurfaceY - offsetY)));
            SetCharacterAlpha(alpha);
            yield return null;
        }
        SetCharacterAlpha(0.0f);
        DisableVisibility();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(CollapsePortal());
    }

    IEnumerator CollapsePortal()
    {
        yield return new WaitForSeconds(0.5f);
        float initialScale = spawnedPortal.transform.localScale.x;
        while (spawnedPortal.transform.localScale.x > 0.0001f)
        {
            float newScale = spawnedPortal.transform.localScale.x - (collapseSpeed * Time.deltaTime);
            spawnedPortal.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        spawnedPortal.SetActive(false);
        uiManager.ShowEnd();
    }


    void SetCharacterAlpha(float alpha)
    {
        foreach (Renderer renderer in characterRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }
    }

    void DisableVisibility()
    {
        foreach (Renderer renderer in characterRenderers)
        {
            renderer.enabled = false;
        }
    }
}

