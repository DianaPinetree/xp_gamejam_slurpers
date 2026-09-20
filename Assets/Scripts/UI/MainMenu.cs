using System.Collections;
using DG.Tweening;
using UnityEngine;

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

    public void ExitGame_ButtonCallback()
    {
         Application.Quit();
    }
}
