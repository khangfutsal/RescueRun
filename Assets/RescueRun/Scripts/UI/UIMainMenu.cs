using System.Collections;
using System.Collections.Generic;
using Lib;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace RescueRun
{
    public class UIMainMenu : BaseUI
    {
        [SerializeField] private Button btnGo;
        [SerializeField] private TextMeshProUGUI coinText;

        [SerializeField] private GameObject prefBuffUI;
        [SerializeField] private Transform horBuffUI;
        [SerializeField] private int countBuffUI;
        [SerializeField] private BuffDatabase buffDatabase;



        private void Start()
        {
            btnGo.onClick.AddListener(ButtonGo);
        }

        public void Initialize()
        {
            buffDatabase = GameController.Instance.GetListBuff();
            SpawnBuffUI();
        }

        public void SpawnBuffUI()
        {
            if (buffDatabase == null || buffDatabase.Buffs == null || buffDatabase.Buffs.Count == 0)
            {
                Debug.LogError("BuffDatabase is null or empty.");
                return;
            }

            int maxBuffs = Mathf.Min(countBuffUI, buffDatabase.Buffs.Count);
            for (int i = 0; i < maxBuffs; i++)
            {
                GameObject obj = Instantiate(prefBuffUI, transform.position, Quaternion.identity, horBuffUI);
                BuffUI buffUI = obj.GetComponent<BuffUI>();

                if (buffUI == null)
                {
                    Debug.LogWarning("Spawned object does not contain BuffUI component.");
                    continue;
                }

                UpdateBuffUI(buffDatabase.Buffs[i].name, 0, buffUI);
            }
        }

        public void UpdateBuffUI(string nameBuff, int idx, BuffUI buffUI)
        {
            var buffs = buffDatabase.GetBuffByName(nameBuff);
            if (buffs == null || idx >= buffs.currencies.Count)
            {
                Debug.LogWarning($"Invalid Buff or index out of range for Buff: {nameBuff}");
                buffUI.DisableButton();
                return;
            }

            string textBuff = buffs.name;
            string textValue = buffs.currencies[idx].value.ToString();
            string textCoin = buffs.currencies[idx].coin.ToString();

            buffUI.AssignData(textBuff, textValue, textCoin);
            buffUI.SubscribeEvent(textBuff, idx);

            buffUI.onBought?.AddListener((string name, int nextIdx, BuffUI curBuffUI) =>
            {
                BuffData buffData = buffDatabase.GetBuffByName(name);
                if (buffData == null || nextIdx >= buffData.currencies.Count)
                {
                    curBuffUI.DisableButton();
                }
                else
                {
                    UpdateBuffUI(name, nextIdx, curBuffUI);
                }
            });
        }


        public void ButtonGo()
        {
            UIController.Instance.GetUIGameplay().ShowCountdownPanel();
            GameSceneManager.Instance.LoadScene("Gameplay");
            Hide();
        }

        public void SetCoinText(string coin)
        {
            coinText.text = coin;
        }


    }
}

