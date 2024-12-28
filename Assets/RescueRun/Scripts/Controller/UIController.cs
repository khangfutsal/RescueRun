using System.Collections;
using System.Collections.Generic;
using Lib;
using RescueRun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [Header("Reference")]
    [SerializeField] private UIGameplay UIGameplay;
    [SerializeField] private UIMainMenu UIMainMenu;
    [SerializeField] private UILoading UILoading;

    public Canvas canvas;
    public UILoading GetUILoading() { return UILoading; }
    public UIGameplay GetUIGameplay() { return UIGameplay; }
    public UIMainMenu GetUIMainMenu() { return UIMainMenu; }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }


    public void Initialize()
    {
        UIGameplay.Initialize(canvas);
        UIMainMenu.Initialize();
        UILoading.Hide();
    }

}
