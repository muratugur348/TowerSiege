﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class RUIMaster : MonoBehaviour
{
    #region //----> Variable
    [Tooltip("Erişe bilmek için kullanılan Id")]
    public string Id;
    [Tooltip("Açılış ve kapanış Animasyonu")]
    public AnimationType animationType;
    public float animationTime = .5f;
    [Tooltip("Açıldığı an Vermiş Olduğunuz fonksiyon Çalışır .")]
    public UnityEvent onBegin;
    [Tooltip("Kapandığı zaman Verilen Fonksiyon Çalışır .")]
    public UnityEvent onEnd;
    [HideInInspector]
    public UnityEvent dynamicOnEnd;
    [HideInInspector]
    public CanvasGroup myCanvasGroup;
    [HideInInspector]
    public RectTransform myRectTransform;
    #endregion
    #region //----> Method
    private void Awake()
    {
        myCanvasGroup = GetComponent<CanvasGroup>();
        myRectTransform = GetComponent<RectTransform>();
    }
    /// <summary>
    /// Open İşlemi Sayfanın Açılmasını Sağlayacak olan İşlem Her Sayfanın kendi içinde olacak ama Rak
    /// </summary>
    public virtual void Open(float closeTime = 0)
    {
        switch (animationType)
        {
            case AnimationType.alpha:
                LeanTween.moveLocalY(gameObject, 0, 0);
                LeanTween.alphaCanvas(myCanvasGroup, 1, animationTime).setOnComplete(AnimationBegin);
                break;
            case AnimationType.horizontal:
                if (myCanvasGroup.alpha < 1)
                {
                    myCanvasGroup.alpha = 1;
                }
                LeanTween.move(myRectTransform, Vector2.zero, animationTime).setOnComplete(AnimationBegin);
                break;
            case AnimationType.vertical:
                if (myCanvasGroup.alpha < 1)
                {
                    myCanvasGroup.alpha = 1;
                }
                LeanTween.move(myRectTransform, Vector2.zero, animationTime).setOnComplete(AnimationBegin);
                break;
            case AnimationType.popupAlert:
                LeanTween.scale(gameObject, Vector3.one, animationTime).setEaseInOutBack().setOnComplete(AnimationBegin);
                LeanTween.alphaCanvas(myCanvasGroup, 1, animationTime).setOnComplete(AnimationBegin);
                break;
            case AnimationType.popup:
                LeanTween.scale(gameObject, Vector3.one, animationTime).setEaseInOutBack().setOnComplete(AnimationBegin);
                LeanTween.alphaCanvas(myCanvasGroup, 1, animationTime).setOnComplete(AnimationBegin);
                break;
        }
        if (closeTime > 0)
        {
            Invoke("Close", closeTime);
        }   
    }
    /// <summary>
    /// 
    /// </summary>
    public virtual void AnimationBegin()
    {
        onBegin?.Invoke();
        myCanvasGroup.interactable = true;
    }
    /// <summary>
    /// Close Kapatma İşlemi
    /// </summary>
    public virtual void Close()
    {
        switch (animationType)
        {
            case AnimationType.alpha:
                LeanTween.alphaCanvas(myCanvasGroup, 0, animationTime).setOnComplete(AnimationFinish);
                break;
            case AnimationType.horizontal:
                LeanTween.moveLocalY(gameObject, RCanvas.canvasSize.y, animationTime).setOnComplete(AnimationFinish);
                break;
            case AnimationType.vertical:
                LeanTween.moveLocalX(gameObject, RCanvas.canvasSize.x, animationTime).setOnComplete(AnimationFinish);
                break;
            case AnimationType.popupAlert:
                LeanTween.scale(gameObject, new Vector3(.5f, .5f, .5f), animationTime).setEaseInOutBack().setOnComplete(AnimationFinish);
                LeanTween.alphaCanvas(myCanvasGroup, 0, animationTime).setOnComplete(AnimationFinish);
                break;
            case AnimationType.popup:
                LeanTween.scale(gameObject, Vector3.zero, animationTime).setEaseInOutBack().setOnComplete(AnimationFinish);
                LeanTween.alphaCanvas(myCanvasGroup, 0, animationTime).setOnComplete(AnimationFinish);
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public virtual void ShotDown()
    {
        switch (animationType)
        {
            case AnimationType.alpha:
                myCanvasGroup.alpha = 0;
                myCanvasGroup.interactable = false;
                LeanTween.moveLocalY(gameObject, RCanvas.canvasSize.y * 2, 0);
                break;
            case AnimationType.horizontal:
                myCanvasGroup.interactable = false;
                LeanTween.moveLocalY(gameObject, RCanvas.canvasSize.y * 2, 0);
                break;
            case AnimationType.vertical:
                myCanvasGroup.interactable = false;
                LeanTween.moveLocalX(gameObject, RCanvas.canvasSize.x * 2, 0);
                break;
            case AnimationType.popup:
                LeanTween.scale(gameObject, Vector3.zero, 0);
                break;
            default:
                myCanvasGroup.alpha = 0;
                myCanvasGroup.interactable = false;
                LeanTween.scale(gameObject, Vector3.zero, 0);
                break;
        }
    }
    /// <summary>
    /// close Kapandığında Tetiklenecke olan Fonsiyon
    /// </summary>
    public virtual void AnimationFinish()
    {
        onEnd.Invoke();
        dynamicOnEnd.Invoke();
        dynamicOnEnd.RemoveAllListeners();
        myCanvasGroup.interactable = false;
        myCanvasGroup.alpha = 0;
        ShotDown();
    }
    #endregion
}
public enum AnimationType
{
    alpha,
    horizontal,
    vertical,
    popup,
    popupAlert
}
