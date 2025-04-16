using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class UINavigationItemEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configurations")]
    [SerializeField] private float defaultYPosition;
    [SerializeField] private float hoverYPosition;
    [SerializeField] private float tweenDuration;

    [SerializeField] private Color defaultOverlayColor;
    [SerializeField] private Color hoverOverlayColor;
    [SerializeField] private Color defaultIconColor;
    [SerializeField] private Color hoverIconColor;
    [SerializeField] private Color defaultTextColor;
    [SerializeField] private Color hoverTextColor;
    [Header("References")]
    [SerializeField] private Image overlayImage;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;

    private RectTransform rectTransform;
    private bool selected = false;

    private void OnEnable()
    {
        UINavigationItemController.OnItemClicked += HandleNavigationItemClicked;
    }

    private void OnDisable()
    {
        UINavigationItemController.OnItemClicked -= HandleNavigationItemClicked;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selected) { return; }

        RenderHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selected) { return; }

        RenderDefault();
    }

    private void RenderDefault()
    {
        rectTransform.DOLocalMoveY(defaultYPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(defaultOverlayColor, tweenDuration, true, true);
        icon.CrossFadeColor(defaultIconColor, tweenDuration, true, true);
        label.CrossFadeColor(defaultTextColor, tweenDuration, true, true);
    }

    private void RenderHover()
    {
        rectTransform.DOLocalMoveY(hoverYPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(hoverOverlayColor, tweenDuration, true, true);
        icon.CrossFadeColor(hoverIconColor, tweenDuration, true, true);
        label.CrossFadeColor(hoverTextColor, tweenDuration, true, true);
    }

    private void HandleNavigationItemClicked(Transform self)
    {
        // render default just in case
        RenderDefault();

        // de-select just in case
        selected = false;

        if (!self.Equals(transform)) { return; }

        // re-select item
        selected = true;

        // render hover state
        RenderHover();
    }
}
