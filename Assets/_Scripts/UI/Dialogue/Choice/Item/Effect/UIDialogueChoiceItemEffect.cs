using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIDialogueChoiceItemEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configurations")]
    [SerializeField] private float defaultXPosition;
    [SerializeField] private float hoverXPosition;
    [SerializeField] private float tweenDuration;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;

    private Image overlayImage;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        overlayImage = GetComponent<Image>();
        
        // call this on start to set default values
        RenderDefault();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RenderHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RenderDefault();
    }

    private void RenderDefault()
    {
        rectTransform.DOLocalMoveX(defaultXPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(defaultColor, tweenDuration, true, true);
    }

    private void RenderHover()
    {
        rectTransform.DOLocalMoveX(hoverXPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(hoverColor, tweenDuration, true, true);
    }
}
