using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

public class AREggPlacement : MonoBehaviour
{
    public GameObject objectPrefab;
    GameObject spawned_object;
    bool object_spawned;
    public ARRaycastManager raycastManager;
    List<ARRaycastHit> hits=new List<ARRaycastHit>();

    void Start()
    {
        object_spawned = false;
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitpose=hits[0].pose;
                if(!object_spawned)
                {
                    spawned_object = Instantiate(objectPrefab, hitpose.position, hitpose.rotation);
                    object_spawned = true;
                }
                else{
                    spawned_object.transform.position=hitpose.position;
                }
                
            }
        }
    }
}