using System.Collections.Generic;
using UnityEngine;

public enum DecorationType
{
    Floor,
    Walls,
    Ceiling
}
[CreateAssetMenu(fileName ="new decoration", menuName = "Decoration Definition")]
public class Decoration : ScriptableObject
{
    public Sprite decorationImage;
    public string decorationName;
    public DecorationType type;
    public GameObject decorationPrefab;

    public GameObject GetDecoration(Vector3 point)
    {
        return GameObject.Instantiate(decorationPrefab, point, Quaternion.identity);
    }
}