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
    public float offsetY = 0.1f;
    private ARPlaneManager arPlaneManager;
    private float arSurfaceY;
    public GameObject spawnedPortal;
    private Renderer[] characterRenderers;

    public Vector3 portalScale = new Vector3(2f, 2f, 2f);
    public float disappearSpeed = 0.1f;
    public float collapseSpeed = 1.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterRenderers = GetComponentsInChildren<Renderer>();
        arPlaneManager = arPlaneManager = FindObjectOfType<ARPlaneManager>();
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
            yield return new WaitForSeconds(0.5f);
        }
    }
    void OnJump()
    {
        if (!hasSpawnedPortal && portalPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, arSurfaceY - offsetY, transform.position.z);
            spawnedPortal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
            spawnedPortal.transform.localScale = portalScale;
            hasSpawnedPortal = true;
            StartCoroutine(MoveDownAndDisappear());
        }
    }
    IEnumerator MoveDownAndDisappear()
    {
        yield return new WaitForSeconds(0.5f);
        float initialY = transform.position.y;
        while (transform.position.y > arSurfaceY - offsetY)
        {
            transform.position += Vector3.down * disappearSpeed * Time.deltaTime;
            float alpha = Mathf.Clamp01((transform.position.y - (arSurfaceY - offsetY)) / (initialY - (arSurfaceY - offsetY)));
            SetCharacterAlpha(alpha);
            yield return null;
        }
        SetCharacterAlpha(0.0f);
        gameObject.SetActive(false);
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
}

