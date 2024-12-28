using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RescueRun
{
    public class UILose : BaseUI
    {
        [SerializeField] private Button btnBack;

        private void Start()
        {
            btnBack.onClick.AddListener(ButtonBack);
        }

        public void ButtonBack()
        {
            StopAllCoroutines();
            Hide();
            Controller.SetControl(Watermelon.InputType.Keyboard);
            LevelController.Instance.DestroyLevel();
            UIController.Instance.GetUIGameplay().GetButtonSetting().gameObject.SetActive(false);
            UIController.Instance.GetUIGameplay().HideCountdownPanel();
            UIController.Instance.GetUIMainMenu().Show();
            UIController.Instance.GetUIGameplay().GetSlider().gameObject.SetActive(true);
            UIController.Instance.GetUIGameplay().GetTextCountdown().gameObject.SetActive(false);

            GameController.Instance.SetCurrentState(GameState.MainMenu);

            GameSceneManager.Instance.UnloadScene("Gameplay");
        }
    }

}
