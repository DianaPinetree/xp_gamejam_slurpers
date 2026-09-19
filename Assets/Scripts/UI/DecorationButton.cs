using System;
using System.Collections.Generic;
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
        transform.DOLocalRotate( new Vector3(0, 0, Random.Range(-10, 10f)), 0.1f).SetEase(Ease.InCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutCubic);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOPunchScale(Vector3.one * Random.Range(0.1f, 0.3f), 0.2f).SetEase(Ease.OutQuad);
        GameManager.Instance.SendDecorationSelect_Action(decoration);
    }
}
