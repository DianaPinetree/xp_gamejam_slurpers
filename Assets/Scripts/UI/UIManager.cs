using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform decorationsArea;
    [SerializeField] private DecorationButton decorationButtonInstance;
    void Start()
    {
        var allDecors = GameManager.Instance.decorationData;

        foreach (var decor in allDecors)
        {
            DecorationButton button = Instantiate(decorationButtonInstance, decorationsArea);
            button.SetDecoration(decor);
            button.enabled = true;
        }
        
        if (decorationButtonInstance.isActiveAndEnabled)
        {
            decorationButtonInstance.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
