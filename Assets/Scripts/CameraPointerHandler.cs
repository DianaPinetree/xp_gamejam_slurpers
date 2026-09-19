using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraPointerHandler : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private Camera cam;
    private Vector3 hitPoint;
    public RaycastHit mainHit;

    public Vector3 HitPoint
    {
        get
        {
            if (!Valid)
                return Vector3.zero;
            return hitPoint;
        }
    }

    public bool Valid { get; private set; }

    private RaycastHit[] hits;

    private void Start()
    {
        hits = new RaycastHit[2];
        cam = Camera.main;
    }

    private void Update()
    {
        if (cam == null)
        {
            Debug.LogError("Camera null oopsie");
        }

        Ray r = cam.ScreenPointToRay(Mouse.current.position.value);

        if (hits != null)
        {
            int hitCount = Physics.RaycastNonAlloc(r, hits, mask);
            Valid = hitCount > 0;
            if (Valid)
            {
                hitPoint = hits[0].point;
                mainHit = hits[0];
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Valid)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hitPoint, 0.2f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(HitPoint, 0.2f);
        }
    }
}