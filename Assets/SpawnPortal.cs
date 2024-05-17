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
    public float collapseSpeed = 8.0f;
    public UIManager uiManager;
    public GameObject character;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterRenderers = GetComponentsInChildren<Renderer>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        StartCoroutine(UpdateARSurfaceY());
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("jump_down"))
        {
            OnJump();
        }
        else
        {
            hasSpawnedPortal = false;
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
    IEnumerator DelayedOnJump()
    {
        yield return new WaitForSeconds(0.1f);
        OnJump();
    }
    void OnJump()
    {
        if (!hasSpawnedPortal && portalPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x-0.03f, arSurfaceY - offsetY, transform.position.z - 0.03f);
            spawnedPortal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
            spawnedPortal.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            hasSpawnedPortal = true;
            StartCoroutine(PopUpPortal());
        }
    }

    IEnumerator PopUpPortal()
    {
        while (spawnedPortal.transform.localScale.x < 0.06f)
        {
            float newScale = spawnedPortal.transform.localScale.x + (collapseSpeed * Time.deltaTime);
            spawnedPortal.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveDownAndDisappear());
    }
    IEnumerator MoveDownAndDisappear()
    {
        float initialY = transform.position.y;
        while (transform.position.y > arSurfaceY - offsetY)
        {
            Debug.Log("CHARACTER POSITION: " + transform.position);
            Debug.Log("PORTAL POSITION: " + spawnedPortal.transform.position);
            transform.position += Vector3.down * disappearSpeed * Time.deltaTime;
            float alpha = Mathf.Clamp01((transform.position.y - (arSurfaceY - offsetY)) / (initialY - (arSurfaceY - offsetY)));
            SetCharacterAlpha(alpha);
            yield return null;
        }
        SetCharacterAlpha(0.0f);
        //gameObject.SetActive(false);
        DisableVisibility();
        StartCoroutine(CollapsePortal());
    }

    IEnumerator CollapsePortal()
    {
        float initialScale = spawnedPortal.transform.localScale.x;
        while (spawnedPortal.transform.localScale.x > 0.01f)
        {
            float newScale = spawnedPortal.transform.localScale.x - (collapseSpeed * Time.deltaTime);
            spawnedPortal.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }
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

