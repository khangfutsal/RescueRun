using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RescueRun
{
    public class UIPause : BaseUI
    {
        [SerializeField] private Button btnBack;
        [SerializeField] private Button btnContinue;
        private void Start()
        {
            btnBack.onClick.AddListener(ButtonBack);
            btnContinue.onClick.AddListener(ButtonContinue);
        }

        public void ButtonBack()
        {
            StopAllCoroutines();
            Hide();
            LevelController.Instance.DestroyLevel();
            UIController.Instance.GetUIGameplay().GetButtonSetting().gameObject.SetActive(false);
            UIController.Instance.GetUIGameplay().HideCountdownPanel();
            UIController.Instance.GetUIMainMenu().Show();
            UIController.Instance.GetUIGameplay().GetSlider().gameObject.SetActive(true);
            UIController.Instance.GetUIGameplay().GetTextCountdown().gameObject.SetActive(false);

            GameController.Instance.SetCurrentState(GameState.MainMenu);

            GameSceneManager.Instance.UnloadScene("Gameplay");
        }

        public void ButtonContinue()
        {
            Time.timeScale = 1;
            GameController.Instance.SetCurrentState(GameState.Start);
            transform.gameObject.SetActive(false);

        }
    }

}
