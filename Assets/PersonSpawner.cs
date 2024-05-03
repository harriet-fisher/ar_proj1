using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PersonSpawner : MonoBehaviour
{
   public GameObject personPrefab;

    public void SpawnAdditionalPrefab(Vector3 position, Quaternion rotation)
    {
        Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
        Quaternion objectRotation = Quaternion.Euler(0f, cameraRotation.eulerAngles.y, 0);
        GameObject spawned = Instantiate(personPrefab, position, objectRotation);
        spawned.transform.localScale = new Vector3(0.065f, 0.065f, 0.065f);
    }
}