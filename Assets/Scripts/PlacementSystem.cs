using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

// Uses CameraPointerHandler and decoration system to place objects in the environment
public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Color indicatorBadColor = Color.red;
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private GameObject indicator;
    private CameraPointerHandler pointerHandler;
    private GameObject decorationObject;
    private Vector3 localBoundBoxCenter;
    private Decoration currentDecorData;

    private bool validPlacement;

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

    private void Start()
    {
        if (indicator)
        {
            indicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (!PlacingItem) return;

        validPlacement = pointerHandler.Valid;
        decorationObject.SetActive(pointerHandler.Valid);
        if (pointerHandler.mainHit.transform)
        {
            Placeable placeable = pointerHandler.mainHit.transform.GetComponent<Placeable>();
            if (placeable && !placeable.allowsOnTop)
            {
                validPlacement = false;
            }
        }

        float dot = Vector3.Dot(pointerHandler.mainHit.normal, decorationObject.transform.up);
        if (currentDecorData.type == DecorationType.Walls)
        {
            if (dot > 0.95f)
            {
                validPlacement = false;
            }
        }
        else
        {
            if (dot < 0.95f)
            {
                validPlacement = false;
            }
        }

        if (indicator)
        {
            indicator.SetActive(pointerHandler.Valid);

            // Please dont do this, its game jam code
            indicator.GetComponent<MeshRenderer>().material.color = validPlacement ? Color.white : indicatorBadColor;
        }

        if (pointerHandler.Valid)
        {
            RaycastHit hit = pointerHandler.mainHit;
            Vector3 objectTarget = hit.point + hit.normal * 1.5f;

            if (indicator)
            {
                indicator.transform.position = hit.point + hit.normal * 0.01f;
                indicator.transform.rotation = Quaternion.LookRotation(-hit.normal, transform.up);
            }

            if (currentDecorData.type == DecorationType.Walls)
            {
                Quaternion targetRot =
                    Quaternion.FromToRotation(decorationObject.transform.right, -pointerHandler.mainHit.normal);
                decorationObject.transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
            }

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
            if (!pointerHandler.Valid) // if there are no hits
            {
                ReleaseAndPlaceItem();
            }
            else if (!validPlacement)
            {
                decorationObject.transform.DOShakeRotation(Random.Range(0.1f, 0.2f), new Vector3(5f, 5f, 5f))
                    .SetEase(Ease.OutCirc).OnComplete(() => decorationObject.transform.rotation = Quaternion.identity);
            }
            else
            {
                ReleaseAndPlaceItem();
            }
        }
    }

    private void ReleaseAndPlaceItem()
    {
        if (!PlacingItem || decorationObject == null) return;
        PlacingItem = false;
        indicator.SetActive(false);

        if (!pointerHandler.Valid)
        {
            Destroy(decorationObject);
            return;
        }

        SetDecorationLayer(LayerMask.NameToLayer("Default"));
        if (currentDecorData.type == DecorationType.Walls)
        {
            // decorationObject.transform.rotation =
            //     Quaternion.FromToRotation(-decorationObject.transform.right, pointerHandler.mainHit.normal);
            decorationObject.transform.right = -pointerHandler.mainHit.normal;
            decorationObject.transform.position = pointerHandler.HitPoint + pointerHandler.mainHit.normal * 0.01f;
        }
        else
        {
            decorationObject.transform.position = pointerHandler.HitPoint;
        }

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
        currentDecorData = decor;
        decorationObject = decor.GetDecoration(pointerHandler.HitPoint);
        if (!decorationObject.TryGetComponent<Placeable>(out Placeable pc))
        {
            decorationObject.AddComponent<Placeable>();
        }

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