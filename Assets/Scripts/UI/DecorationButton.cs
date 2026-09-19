using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DecorationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Decoration decoration;

    [SerializeField] private Image decorationImage;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (decoration != null)
        {
            SetDecoration(decoration);
        }
    }

    public void SetDecoration(Decoration newDecor)
    {
        decoration = newDecor;
        SetupButton();
    }

    private void SetupButton()
    {
        decorationImage.sprite = decoration.decorationImage;
        text.text = decoration.decorationName;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOLocalRotate( new Vector3(0, 0, 10f), 0.4f).SetEase(Ease.InCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutCubic);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOPunchScale(Vector3.one * Random.Range(1f, 1.2f), 0.2f).SetEase(Ease.OutQuad);
    }
}
