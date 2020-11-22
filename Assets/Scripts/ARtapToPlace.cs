using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class ARtapToPlace : MonoBehaviour
{

    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    public List<GameObject> placedPrefabList = new List<GameObject>();

    [SerializeField]
    private int maxPrefabSpawnCount = 0;
    private int placedPrefabCount;

    [SerializeField]
    private GameObject gameObjectToInstantiate;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began){
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                placedPrefabList.Add(spawnedObject);
                placedPrefabCount++;
            }

        }
    }

    public void SetPrefabType(GameObject prefabType)
    {
        gameObjectToInstantiate = prefabType;
    }

    public void Remember(GameObject glow)
    {
        spawnedObject = Instantiate(gameObjectToInstantiate, hits[0].pose.position,hits[0].pose.rotation);
        placedPrefabList.Add(spawnedObject);
        placedPrefabCount++;
    }

    public GameObject getGameObject()
    {
        return placedPrefabList[0];
    }

}
