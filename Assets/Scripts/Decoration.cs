using UnityEngine;

[CreateAssetMenu(fileName ="new decoration", menuName = "Decoration Definition")]
public class Decoration : ScriptableObject
{
    public Sprite decorationImage;
    public string decorationName;
    public GameObject decorationPrefab;

    public GameObject GetDecoration(Vector3 point)
    {
        return GameObject.Instantiate(decorationPrefab, point, Quaternion.identity);
    }
}