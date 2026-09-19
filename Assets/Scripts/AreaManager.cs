using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    private List<GameObject> placedDecor = new List<GameObject>();


    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        if (placedDecor.Count > 0)
        {
            for (int i = placedDecor.Count -1; i >= 0; i--)
            {
                Destroy(placedDecor[i].gameObject);
            }
            placedDecor.Clear();
        }

        StartCoroutine(DoorCloseAfter(2f));
    }

    private IEnumerator DoorCloseAfter(float time)
    {
        yield return new WaitForSeconds(2f);
        doorAnimator?.SetTrigger("Close");
    }
}
