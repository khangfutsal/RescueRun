using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RescueRun
{
    public class AnimalSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        private void Update()
        {
            Vector3 v = transform.position - Camera.main.transform.position;
            transform.rotation = Quaternion.LookRotation(v);
        }

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Start()
        {
            HideSlider();
        }

        public void ShowSlider()
        {
            slider.gameObject.SetActive(true);
        }

        public void HideSlider()
        {
            slider.gameObject.SetActive(false);
        }

        public void SetSlider(float value)
        {
            slider.value = value;
        }

        public bool CheckIsSliderFull()
        {
            return slider.value >= slider.maxValue;
        }
    }
}

