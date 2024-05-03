using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EggSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float rotationDuration = 5f;
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool hasSpawned = false;
    private GameObject spawnedObject;
    private bool isRotating = false;
    public bool isActive = false;

    public PersonSpawner additionalSpawner;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (isActive && !hasSpawned && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
                Quaternion objectRotation = Quaternion.Euler(-90f, cameraRotation.eulerAngles.y, -180f);

                MeshRenderer meshR = objectToSpawn.GetComponent<MeshRenderer>();
                float objectHeight = meshR != null ? meshR.bounds.size.y : 0f;
                Vector3 spawnPosition = new Vector3(hitPose.position.x, hitPose.position.y + objectHeight/2, hitPose.position.z);

                spawnedObject = Instantiate(objectToSpawn, spawnPosition, objectRotation);
                hasSpawned = true;
                additionalSpawner.SpawnAdditionalPrefab(hitPose.position, objectRotation);
                Transform hingeTransform = spawnedObject.transform.Find("hinge");
                if (hingeTransform != null)
                {
                    StartCoroutine(RotateHinge(hingeTransform));
                }
                else
                {
                    Debug.LogError("Hinge object not found!");
                }
            }
        }
    }

IEnumerator RotateHinge(Transform hinge)
    {
        Quaternion startRotation = hinge.localRotation;
        Quaternion endRotation = Quaternion.Euler(-35f, -90f, -90f);
        float timeElapsed = 0;
        Debug.Log("Starting rotation");
        
        while (timeElapsed < rotationDuration)
        {
            hinge.localRotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        hinge.localRotation = endRotation;
    }

public void ActivateSpawning()
{
    isActive = true;
}

}