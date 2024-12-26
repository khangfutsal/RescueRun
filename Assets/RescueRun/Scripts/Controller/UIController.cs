using System.Collections;
using System.Collections.Generic;
using Lib;
using RescueRun;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [Header("Reference")]
    [SerializeField] private UIGameplay UIGameplay;
    public Canvas canvas;

    public UIGameplay GetUIGameplay() { return UIGameplay; }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Initialize()
    {
        UIGameplay.Initialize(canvas);

    }

}
