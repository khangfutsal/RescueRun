using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Watermelon;
using Joystick = Watermelon.Joystick;
namespace RescueRun
{
    public class UIGameplay : BaseUI
    {
        [SerializeField] private Joystick joystick;

        [Header("About Slider")]
        [SerializeField] private Slider slider;
        [SerializeField] private Image imgFill;
        [SerializeField] private TextMeshProUGUI textStamina;
        [SerializeField] private Color colorUseless;
        [SerializeField] private Color colorUsing;
        public void Initialize(Canvas canvas)
        {
            joystick.Initialise(canvas);
        }

        private void Start()
        {
            slider = GetComponentInChildren<Slider>();

            Transform fillArea = slider.transform.Find("FillArea");
            imgFill = fillArea.transform.Find("Fill").GetComponent<Image>();
            textStamina = fillArea.GetComponentInChildren<TextMeshProUGUI>();

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



        #endregion
    }
}

