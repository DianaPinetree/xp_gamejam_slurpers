using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPoster : MonoBehaviour
{
    private static int lastIndex = -1;
    [SerializeField] private List<Material> posterMats;

    private void Start()
    {
        MeshRenderer r = GetComponent<MeshRenderer>();
        int pickedIndex = Random.Range(0, posterMats.Count);
        while (pickedIndex == lastIndex)
        {
            pickedIndex = Random.Range(0, posterMats.Count);
        }
        r.material = posterMats[pickedIndex];

        lastIndex = pickedIndex;
    }
}