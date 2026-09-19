using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

// Uses CameraPointerHandler and decoration system to place objects in the environment
public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private GameObject indicator;
    private CameraPointerHandler pointerHandler;
    private GameObject decorationObject;
    private Vector3 localBoundBoxCenter;
    
    public bool PlacingItem { get; private set; }

    private void Awake()
    {
        pointerHandler = GetComponent<CameraPointerHandler>();
        PlacingItem = false;
    }

    private void OnEnable()
    {
        GameManager.setActivePlacementDecoration += ActiveDecorationChange;
    }

    private void OnDisable()
    {
        GameManager.setActivePlacementDecoration -= ActiveDecorationChange;
    }

    private void Update()
    {
        if (!PlacingItem) return;
        
        decorationObject.SetActive(pointerHandler.Valid);
        if (pointerHandler.Valid)
        {
            RaycastHit hit = pointerHandler.mainHit;
            Vector3 objectTarget = hit.point + hit.normal * 1.5f;

            decorationObject.transform.position =
                Vector3.Lerp(decorationObject.transform.position, objectTarget, Time.deltaTime * followSpeed);

            Vector3 worldCenter = decorationObject.transform.TransformPoint(localBoundBoxCenter);
            Vector3 targetDir = objectTarget - worldCenter;
            targetDir.Normalize();
            Quaternion rotation = Quaternion.FromToRotation(transform.up, targetDir);

            if (Mouse.current.rightButton.isPressed)
            {
                decorationObject.transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime), Space.Self);
            }

            if (Mouse.current.scroll.value.magnitude > 0)
            {
                if (Mouse.current.scroll.up.magnitude > 0.01f)
                {
                    decorationObject.transform.localScale += Vector3.one * 10f * Time.deltaTime;
                }
                else if (Mouse.current.scroll.down.magnitude > 0.01f)
                {
                    decorationObject.transform.localScale -= Vector3.one * 10f * Time.deltaTime;
                }
            }
        }

        if (Mouse.current.leftButton.isPressed)
        {
            ReleaseAndPlaceItem();
        }
    }

    private void ReleaseAndPlaceItem()
    {
        if(!PlacingItem || decorationObject == null) return;
        PlacingItem = false;

        if (!pointerHandler.Valid)
        {
            Destroy(decorationObject);
            return;
        }
        SetDecorationLayer(LayerMask.NameToLayer("Default"));
        decorationObject.transform.position = pointerHandler.HitPoint;
        decorationObject = null; // release decoration
    }

    private Vector3 CalculateBoundBoxCenter()
    {
        MeshRenderer[] meshes = decorationObject.GetComponentsInChildren<MeshRenderer>();

        Bounds finalBounds = new Bounds();
        foreach (var mesh in meshes)
        {
            finalBounds.Encapsulate(mesh.bounds);
        }

        return finalBounds.center;
    }

    private void ActiveDecorationChange(Decoration decor)
    {
        PlacingItem = true;
        decorationObject = decor.GetDecoration(pointerHandler.HitPoint);
        var colliders = decorationObject.GetComponentsInChildren<Collider>();

        int layer = LayerMask.NameToLayer("Ignore Raycast");
        SetDecorationLayer(layer);
        localBoundBoxCenter = CalculateBoundBoxCenter();
    }

    private void SetDecorationLayer(int layer)
    {
        var colliders = decorationObject.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.gameObject.layer = layer;
        }
        decorationObject.layer = layer;
    }
}