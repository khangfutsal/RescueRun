using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Watermelon;
using static UnityEngine.Rendering.DebugUI;
using Joystick = Watermelon.Joystick;
namespace RescueRun
{
    public class UIGameplay : BaseUI
    {
        [SerializeField] private UnityEngine.UI.Button btnSetting;

        [SerializeField] private UIWin uiWin;
        [SerializeField] private UILose uiLose;
        [SerializeField] private UIPause uiPause;

        [SerializeField] private Joystick joystick;

        [Header("About Slider")]
        [SerializeField] private Slider slider;
        [SerializeField] private Image imgFill;
        [SerializeField] private TextMeshProUGUI textStamina;
        [SerializeField] private Color colorUseless;
        [SerializeField] private Color colorUsing;

        [Header("About Countdown")]
        [SerializeField] private TextMeshProUGUI textCountdown;
        [SerializeField] private int numberLimit;
        [SerializeField] private GameObject countdownUI;
        [SerializeField] private float timeCountdown;
        public TextMeshProUGUI GetTextCountdown() => textCountdown;
        public UnityEngine.UI.Button GetButtonSetting() => btnSetting;
        public Slider GetSlider() => slider;
        public UIPause GetUIPause() => uiPause;
        public UIWin GetUIWin() => uiWin;
        public UILose GetUILose() => uiLose;
        public Joystick GetJoystick() => joystick;
        public void Initialize(Canvas canvas)
        {
            joystick.Initialise(canvas);

            textCountdown.gameObject.SetActive(false);
        }


        private void Start()
        {
            slider = GetComponentInChildren<Slider>();
            uiWin = GetComponentInChildren<UIWin>();
            uiLose = GetComponentInChildren<UILose>();

            Transform fillArea = slider.transform.Find("FillArea");
            imgFill = fillArea.transform.Find("Fill").GetComponent<Image>();
            textStamina = fillArea.GetComponentInChildren<TextMeshProUGUI>();

            btnSetting.onClick.AddListener(ButtonSetting);
            btnSetting.gameObject.SetActive(false);
            uiPause.Hide();
            uiLose.Hide();
            uiWin.Hide();
            HideCountdownPanel();
        }

        public void ButtonSetting()
        {
            GameController.Instance.SetCurrentState(GameState.Pause);
        }

        #region Slider Component

        public void SetTextSlider(int value, int maxValue)
        {
            textStamina.text = value + "/" + maxValue;
        }

        public void SetMaxSlider(float value)
        {
            slider.maxValue = value;
        }

        public void SetSlider(float value)
        {
            slider.value = value;
        }

        public void ToDefaultColorSlider()
        {
            imgFill.color = colorUsing;
        }

        public void ToUselessColorSlider()
        {
            imgFill.color = colorUseless;
        }

        public IEnumerator StartCountdown()
        {
            slider.gameObject.SetActive(true);
            textCountdown.gameObject.SetActive(true);
            for (int i = numberLimit; i > 0; i--)
            {
                textCountdown.text = i.ToString();
                textCountdown.transform.localScale = Vector3.zero;
                textCountdown.DOFade(1, 0f);
                textCountdown.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                {
                    textCountdown.DOFade(0, 0.5f);
                });
                Debug.Log("" + i);

                yield return new WaitForSeconds(timeCountdown);
            }
            HideCountdownPanel();
            GameController.Instance.SetCurrentState(GameState.Start);

        }

        public void ShowCountdownPanel()
        {
            countdownUI.SetActive(true);
            slider.gameObject.SetActive(false);
            btnSetting.gameObject.SetActive(true);
        }

        public void HideCountdownPanel()
        {
            countdownUI.SetActive(false);
        }



        #endregion
    }
}

