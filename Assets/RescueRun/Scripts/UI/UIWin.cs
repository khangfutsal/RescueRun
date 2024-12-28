using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace RescueRun
{
    public class UIWin : BaseUI
    {
        [SerializeField] private Button btnContinue;
        [SerializeField] private Button btnBack;

        private void Start()
        {
            btnContinue.onClick.AddListener(ButtonContinue);
            btnBack.onClick.AddListener(ButtonBack);
        }

        public void ButtonContinue()
        {
            StartCoroutine(HandleSceneReload());
        }

        private IEnumerator HandleSceneReload()
        {

            int level = 0;
            int inCome = 0;
            if (PlayerPrefs.HasKey("Level"))
            {
                level = PlayerPrefs.GetInt("Level");
            }

            inCome = GameController.Instance.GetIncome();
            UIController.Instance.GetUIGameplay().GetSlider().gameObject.SetActive(false);
            GameController.Instance.AdditionCoin(inCome);
            int resultLevel = level + 1;
            PlayerPrefs.SetInt("Level", resultLevel);
            PlayerPrefs.SetInt("PrevLevel", resultLevel);
            PlayerPrefs.Save();

            // Bắt đầu Unload scene
            var unloadOperation = SceneManager.UnloadSceneAsync("Gameplay");
            while (!unloadOperation.isDone)
            {
                yield return null;
            }

            // Sau khi unload xong, load lại scene
            GameSceneManager.Instance.LoadScene("Gameplay");
            transform.gameObject.SetActive(false);
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

