using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;

    public void StartGame_ButtonCallback()
    {
        GameManager.Instance.StartGame();
        canvas.DOFade(0, 0.8f).OnComplete(() =>
        {
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
            UIManager.ShowGameUI();
        });
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            UIManager.HideGameUI();
            canvas.DOFade(1, 0.8f).OnComplete(() =>
            {
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
            });
        }
    }

    public void ExitGame_ButtonCallback()
    {
         Application.Quit();
    }
}
