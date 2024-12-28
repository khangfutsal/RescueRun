using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace RescueRun
{
    public class UILoading : BaseUI
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textLoading;
        private int maxPercentLoad = 100;

        private void Start()
        {
            //Hide();
            slider.maxValue = maxPercentLoad;
        }

        public void SetSliderLoading(int value)
        {
            slider.value = value;
            textLoading.text = "Loading... " + value.ToString() +"%";
        }

        public void ResetLoadingState()
        {
            SetSliderLoading(0); 
        }

    }

}
