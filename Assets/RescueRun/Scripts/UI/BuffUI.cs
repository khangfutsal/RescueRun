using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
namespace RescueRun
{
    public class BuffUI : MonoBehaviour
    {
        private string nameBuff;
        private int idx;
        [SerializeField] private TextMeshProUGUI textBuff;
        [SerializeField] private Image imageBuff;
        [SerializeField] private TextMeshProUGUI textValue;
        [SerializeField] private TextMeshProUGUI textCoinText;

        [SerializeField] private Button btnBuy;
        public UnityEvent<string, int, BuffUI> onBought;


        public void AssignData(string textBuff, string textValue, string textCoinText)
        {
            this.textBuff.text = textBuff;
            this.textValue.text = textValue;
            this.textCoinText.text = textCoinText;
        }

        public void DisableButton()
        {
            btnBuy.onClick.RemoveAllListeners();
            btnBuy.interactable = false;
        }


        public void SubscribeEvent(string nameBuff, int idx)
        {
            this.nameBuff = nameBuff;
            this.idx = idx;
            btnBuy.onClick.AddListener(() =>
            {
                Debug.Log("Button Clicked");
                int coin = GameController.Instance.GetCoin();
                int result = coin - int.Parse(textCoinText.text);
                Debug.Log(coin - int.Parse(textCoinText.text) + " Coin");
                if (result >= 0)
                {
                    GameController.Instance.MinusCoin(int.Parse(textCoinText.text));
                    AssignValue(nameBuff, int.Parse(textValue.text));
                    onBought?.Invoke(this.nameBuff, ++this.idx, this);
                }
            });
        }

        public void AssignValue(string buff, int value)
        {
            switch (buff)
            {
                case "Speed":
                    GameController.Instance.SetSpeed(value);
                    break;
                case "Stamina":
                    GameController.Instance.SetStamina(value);
                    Slider slider = UIController.Instance.GetUIGameplay().GetSlider();
                    UIController.Instance.GetUIGameplay().SetMaxSlider(value);
                    UIController.Instance.GetUIGameplay().SetTextSlider(value, (int)slider.maxValue);
                    break;
                case "Income":
                    GameController.Instance.SetIncome(value);
                    break;
            }
        }
    }
}

