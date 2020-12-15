using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class ARtapToPlace : MonoBehaviour
{

    private GameObject spawnedObject; //temporary scope within this code
    private ARRaycastManager _arRaycastManager; //touch detect
    private Vector2 touchPosition;  //particular position
    public List<GameObject> placedPrefabList = new List<GameObject>(); //placedgameobjects
    public AudioSource musicSource;

    [SerializeField]
    private int maxPrefabSpawnCount; //count of max objects taht can be placed in ar space
    public int placedPrefabCount; //how many obejcst placed already

    [SerializeField]
    private GameObject gameObjectToInstantiate; //object being instantiated permanent
    static List<ARRaycastHit> hits = new List<ARRaycastHit>(); //positions that have been touched

    [SerializeField]
    public Dictionary<GameObject, Vector2> placedPrefabs = new Dictionary<GameObject, Vector2>();
    private bool canPlaceFlag;
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>(); //instantiate and assigning
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

    private void Update() //periodically runs call getrequest in update
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition) && !canPlaceFlag)
            return;

        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                placedPrefabList.Add(spawnedObject);
                placedPrefabs.Add(gameObjectToInstantiate, touchPosition);
                
                placedPrefabCount++;
            }

        }
    }

    public void SetPrefabType(GameObject prefabType)
    {
       
        if(placedPrefabs.ContainsKey(prefabType))
        {
            canPlaceFlag = false;
        }
        else
        {
            gameObjectToInstantiate = prefabType;
            canPlaceFlag = true;
        }

    }
    public void Remember(GameObject prefabType)
    {
    	SetPrefabType(prefabType);
    	musicSource.Play();
    }

    // public void Remember(GameObject glow)
    // {
    //     spawnedObject = Instantiate(gameObjectToInstantiate, hits[0].pose.position,hits[0].pose.rotation);
    //     placedPrefabList.Add(spawnedObject);
    //     placedPrefabCount++;
    // }

    // public GameObject getGameObject()
    // {
    //     return placedPrefabList[0];
    // }

}
