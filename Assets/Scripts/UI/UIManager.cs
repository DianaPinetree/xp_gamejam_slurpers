using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    [SerializeField] private Transform decorationsArea;
    [SerializeField] private DecorationButton decorationButtonInstance;
    [SerializeField] private Button clearDecorationsButton;
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        if (!_canvasGroup)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _canvasGroup.alpha = 0;
        instance = this;
    }

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
        
        clearDecorationsButton.onClick.AddListener(ClearDecorations);
    }

    private void ClearDecorations()
    {
        GameManager.Instance.ClearLevel();
    }

    private void FadeInUI()
    {
        _canvasGroup.DOFade(1, 0.6f);
    }

    private void FadeOutUI()
    {
        _canvasGroup.DOFade(0, 0.6f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public static void HideGameUI()
    {
        instance.FadeOutUI();
    }
    public static void ShowGameUI()
    {
        instance.FadeInUI();
    }
}
