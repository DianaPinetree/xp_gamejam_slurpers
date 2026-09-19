using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraPointerHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 hitPoint;

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
        Physics.RaycastNonAlloc(r, hits);

        if (hits.Length > 0)
        {
            hitPoint = hits[0].point;
        }
    }

    private void OnDrawGizmos()
    {
        if (hits != null && hits.Length > 0)
        {
            Gizmos.DrawSphere(hitPoint, 0.2f);
        }
    }
}
